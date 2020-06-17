using System;
using System.Collections.Generic;
using HRM.Models.Cores;

namespace HRM.Helpers
{
    public class SalaryHelper
    {
        public static Payroll CalculateSalary(int month, int year, 
            List<Holiday> holidays,
            List<Attendance> attendances, 
            List<DateOff> dateOffs, 
            Contract contract)
        {
            var baseSalary = contract.MonthlyNetSalary ?? 0;
            var totalBonus = contract.MonthlyBonus?.GetAllBonus() ?? 0;
            var totalSalary = baseSalary + totalBonus;
            var socialInsurance = Convert.ToInt32(baseSalary * 0.105);
            
            var totalWorkingHours = CountTotalAttendanceWorkingHour(attendances);
            var totalUnpaidOffHours = CountTotalUnpaidOffHour(dateOffs);
            var totalWorkingDays = CountWorkingDayInMonth(month, year);

            var expectTotalWorkingHours = Constants.WorkingTime.DailyWorkingHours * totalWorkingDays;
            var totalHolidayHours = holidays.Count * Constants.WorkingTime.DailyWorkingHours;

            var totalPaidOffHours = expectTotalWorkingHours - totalWorkingHours - totalHolidayHours - totalUnpaidOffHours;
            var paidPerHours = totalSalary / (totalWorkingDays * Constants.WorkingTime.DailyWorkingHours);
            var totalPaidOffMoney = paidPerHours * totalPaidOffHours;

            var finalSalary = totalSalary - socialInsurance - totalPaidOffMoney;

            var result = new Payroll
            {
                UserId = contract.UserId,
                Bonus = contract.MonthlyBonus, 
                PersonalIncomeTax = CalculatePersonalTax(contract),
                Month = month,
                Year = year,
                BaseSalary = contract.MonthlyNetSalary ?? 0,
                SocialInsurance = socialInsurance,
                TotalSalary = totalSalary,
                TotalExpectWorkingHours = expectTotalWorkingHours,
                PaidLeaveHours = totalPaidOffHours,
                PaidLeaveTotal = totalPaidOffMoney,
                TotalWorkingHours = totalWorkingHours,
                UnPaidLeaveHours = totalUnpaidOffHours,
                
                FinalReceiveSalary = finalSalary,
            };
            return result;
        }

        static int CalculatePersonalTax(Contract contract)
        {
            double result = 0;
            var limitTaxSalary = contract.DependentPerson * Constants.PersonalTax.TAX_PER_DEPENDENT_PERSION +
                              Constants.PersonalTax.MIN_TAX_APPLY_VALUE;
            if (contract.MonthlyNetSalary < limitTaxSalary)
            {
                return 0;
            }

            var taxableSalary = contract.MonthlyNetSalary - limitTaxSalary ?? 0;
            switch (taxableSalary)
            {
                case { } n when n <= 5000000:
                {
                    result = taxableSalary * 0.05;
                    break;
                }
                case { } n when n > 5000000 && n <= 10000000:
                {
                    result = taxableSalary * 0.1 - 250000;
                    break;
                }
                case { } n when n > 10000000 && n <= 18000000:
                {
                    result = taxableSalary * 0.15 - 750000;
                    break;
                }
                case { } n when n > 18000000 && n <= 32000000:
                {
                    result = taxableSalary * 0.2 - 1650000;
                    break;
                }
                case { } n when n > 32000000 && n <= 52000000:
                {
                    result = taxableSalary * 0.25 - 3250000;
                    break;
                }
                case { } n when n > 52000000 && n <= 80000000:
                {
                    result = taxableSalary * 0.3 - 5850000;
                    break;
                }
                case { } n when n > 80000000:
                {
                    result = taxableSalary * 0.35 - 9850000;
                    break;
                }
                default:
                {
                    result = 0;
                    break;
                }
            }
            return Convert.ToInt32(result);
        }

        static int CountWorkingDayInMonth(int month, int year)
        {
            var startD = new DateTime(year, month, 1);
            var endD = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            double calcBusinessDays =
                1 + ((endD - startD).TotalDays * 5 -
                     (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7;

            if (endD.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;
            if (startD.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;

            return Convert.ToInt32(calcBusinessDays);
        }

        static double CountTotalAttendanceWorkingHour(List<Attendance> attendances)
        {
            double count = 0;
            
            foreach (var attendance in attendances)
            {
                foreach (var inOutTime in attendance.Data)
                {
                    count += CountSessionHourInWorkingTime(inOutTime.In, inOutTime.Out);
                }
            }
            
            return count;
        }

        static double CountTotalUnpaidOffHour(List<DateOff> dateOffs)
        {
            double result = 0;
            foreach (var dateOff in dateOffs)
            {
                if (dateOff == null || dateOff.Status != Constants.DateOffStatus.Approve || dateOff.IsUnpaidOff == false) continue;
                result += CountSessionHourInWorkingTime(dateOff.StartOff, dateOff.EndOff);
            }

            return result;
        }

        public static double CountSessionHourInWorkingTime(TimeSpan start, TimeSpan end)
        {
            double result = end.TotalHours - start.TotalHours;
            double startHours = start.TotalHours;
            double endHours = end.TotalHours;
            if (startHours < Constants.WorkingTime.StartWokingTime.TotalHours)
            {
                var earlierHours = Constants.WorkingTime.StartWokingTime.TotalHours - startHours;
                result -= earlierHours;
            }
            
            if (endHours > Constants.WorkingTime.EndWokingTime.TotalHours)
            {
                var laterHours = endHours - Constants.WorkingTime.EndWokingTime.TotalHours;
                result -= laterHours;
            }

            if (startHours <= Constants.WorkingTime.LaunchStartTime.TotalHours &&
                endHours >= Constants.WorkingTime.LaunchEndTime.TotalHours)
            {
                result -= (Constants.WorkingTime.LaunchEndTime.TotalHours -
                           Constants.WorkingTime.LaunchStartTime.TotalHours);
            }
            else
            {
                if (startHours > Constants.WorkingTime.LaunchStartTime.TotalHours)
                {
                    result -= Constants.WorkingTime.LaunchEndTime.TotalHours - startHours;
                }
                
                if (endHours < Constants.WorkingTime.LaunchStartTime.TotalHours)
                {
                    result -= endHours - Constants.WorkingTime.LaunchStartTime.TotalHours;
                }
            }

            return result >= 0 ? result : 0;
        }
    }
}
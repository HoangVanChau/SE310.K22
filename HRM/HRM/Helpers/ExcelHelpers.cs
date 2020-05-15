using System;
using System.Collections.Generic;
using System.Linq;
using HRM.Models.Cores;
using OfficeOpenXml;

namespace HRM.Helpers
{
    public static class ExcelHelpers
    {
        public static List<Attendance> ReadExcelAttendance(ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.First();
            var totalRowCount = worksheet.Dimension.Rows;
            var totalColumnCount = worksheet.Dimension.Columns;

            var listAttendance = new List<Attendance>();
            for (var row = Constants.AttendanceExcel.StartAttendanceDataRow; row <= totalRowCount; row++)
            {
                try
                {
                    var date = DateTime.Parse(worksheet.Cells[row, Constants.AttendanceExcel.DateColumn].Text);
                    var employeeId = uint.Parse(worksheet.Cells[row, Constants.AttendanceExcel.EmployeeIdColumn].Text);
                    var listInOut = new List<InOutTime>();

                    for (var inOutIndex = Constants.AttendanceExcel.StartAttendanceInOutTimeDataColumn; inOutIndex <= totalColumnCount - 1; inOutIndex += 2)
                    {
                        var timeIn = worksheet.Cells[row, inOutIndex].Text;
                        var timeOut = worksheet.Cells[row, inOutIndex + 1].Text;
                        if (timeIn.Equals("") || timeOut.Equals("")) continue;

                        var inOutData = new InOutTime
                        {
                            In = TimeSpan.Parse(timeIn),
                            Out = TimeSpan.Parse(timeOut),
                        };
                        listInOut.Add(inOutData);
                    }

                    var attendance = new Attendance
                    {
                        Unique = new AttendanceUnique
                        {
                            Date = date,
                            EmployeeId = employeeId
                        },
                        Data = listInOut,
                    };
                    listAttendance.Add(attendance);
                }
                catch (Exception e)
                {
                    // ignored
                }
            }

            return listAttendance;
        }
    }
}
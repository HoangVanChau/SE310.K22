using System;

namespace HRM.Constants
{
    public class WorkingTime
    {
        public static TimeSpan StartWokingTime = new TimeSpan(08, 00, 00);
        public static TimeSpan LaunchStartTime = new TimeSpan(12, 00, 00);
        public static TimeSpan LaunchEndTime = new TimeSpan(13, 00, 00);
        public static TimeSpan EndWokingTime = new TimeSpan(17, 00, 00);

        public static int DailyWorkingHours = 8;
    }
}
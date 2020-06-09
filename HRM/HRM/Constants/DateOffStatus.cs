using HRM.Models.Cores;

namespace HRM.Constants
{
    public static class DateOffStatus
    {
        public static Status PendingApprove = new Status
        {
            Label = "Peding Approve",
            Value = 1,
        };
        public static Status Approve = new Status
        {
            Label = "Approve",
            Value = 2,
        };
        public static Status Reject = new Status
        {
            Label = "Reject",
            Value = 3,
        };
        public static Status Cancel = new Status
        {
            Label = "Cancel",
            Value = 4,
        };
    }
}
using System;

namespace HRM.Models.QueryParams
{
    public class DateOffQuery
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public String? UserId { get; set; }
        public String? TeamId { get; set; }
        public int? Status { get; set; }
    }
}
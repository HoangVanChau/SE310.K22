namespace HRM.Models.Cores
{
    public class MonthlyBonus
    {
        public int? Vehicle { get; set; }
        public int? Mobile { get; set; }
        public int? Costume { get; set; }
        public int? House { get; set; }
        public int? ChildSupport { get; set; }
        public int? Diligence { get; set; }
        public int? Responsibilities { get; set; }

        public int GetAllBonus()
        {
            return Vehicle + Mobile + Costume + House + ChildSupport + Diligence + Responsibilities ?? 0;
        }
    }
}
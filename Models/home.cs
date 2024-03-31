namespace G8Cozy.Models
{
    public class home
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public double Price { get; set; }
        public string YearBuilt { get; set; }
        public string SquareFeet { get; set; }
        public string? Keywords { get; set; }

        public string HomeType { get; set; }
        public Int16 Room { get; set; }
        public Int16 Bath { get; set; }

        public string Image { get; set; }
        public bool IsSell { get; set; }
        public bool HavePool { get; set; }
        public bool HaveOnSiteParking { get; set; }
        public bool HavePark { get; set; }
    }
}

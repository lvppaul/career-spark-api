namespace CareerSpark.DataAccessLayer.Helper
{
    // Generic int-key aggregate item used for revenue grouping (year/month/day)
    public class RevenueGroupInt
    {
        public int Key { get; set; }
        public decimal Total { get; set; }
    }
}

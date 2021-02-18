namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class PriceExtensions
    {
        public static string FormatCost(this decimal? cost)
        {
            if (!cost.HasValue) return string.Empty;
            return $"£{cost.Value:n0}";
        }
    }
}

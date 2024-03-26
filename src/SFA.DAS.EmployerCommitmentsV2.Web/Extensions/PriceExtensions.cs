namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

public static class PriceExtensions
{
    public static string FormatCost(this decimal? cost)
    {
        return !cost.HasValue ? string.Empty : FormatCost(cost.Value);
    }

    public static string FormatCost(this decimal cost)
    {
        return $"£{cost:n0}";
    }

    public static string FormatCost(this int? cost)
    {
        return !cost.HasValue ? string.Empty : $"£{cost.Value:n0}";
    }
}
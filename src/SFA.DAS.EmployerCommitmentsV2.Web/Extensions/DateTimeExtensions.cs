namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

public static class DateTimeExtensions
{
    public static DateTime FirstOfMonth(this DateTime value)
    {
        return new DateTime(value.Year, value.Month, 1);
    }

    public static string ToGdsFormatLongMonthNameWithoutDay(this DateTime date)
    {
        return date.ToString("MMMM yyyy");
    }
}
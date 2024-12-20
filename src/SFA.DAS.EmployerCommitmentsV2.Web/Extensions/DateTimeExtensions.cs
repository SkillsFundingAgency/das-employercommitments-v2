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

    public static string ToGdsHumanisedDate(this DateTime date)
    {
        string ordinal;

        switch (date.Day)
        {
            case 1:
            case 21:
            case 31:
                ordinal = "st";
                break;
            case 2:
            case 22:
                ordinal = "nd";
                break;
            case 3:
            case 23:
                ordinal = "rd";
                break;
            default:
                ordinal = "th";
                break;
        }

        // Eg 12th January 2024
        return string.Format("{0}{1} {2:MMMM yyyy}", date.Day, ordinal, date);
    }
}
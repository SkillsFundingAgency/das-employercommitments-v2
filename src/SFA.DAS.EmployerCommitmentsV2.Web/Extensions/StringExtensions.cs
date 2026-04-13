namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

public static class StringExtensions
{
    public static string ControllerName(this string value)
    {
        if (value.EndsWith("Controller"))
        {
            return value.Substring(0, value.LastIndexOf("Controller"));
        }

        return value;
    }

    public static T ToEnum<T>(this string value) where T : struct
    {
        return (T)Enum.Parse(typeof(T), value);
    }

    public static string FormatEnumValue(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var displayAttribute = System.Text.RegularExpressions.Regex.Replace(field.Name, "(?<!^)([A-Z])", " $1",
            System.Text.RegularExpressions.RegexOptions.None,
        TimeSpan.FromMilliseconds(100)).ToLower();
        return char.ToUpper(displayAttribute[0]) + displayAttribute.Substring(1);
    }
}
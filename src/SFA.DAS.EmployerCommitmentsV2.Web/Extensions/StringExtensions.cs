using System;

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
}
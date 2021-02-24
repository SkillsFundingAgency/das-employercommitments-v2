using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
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
    }
}
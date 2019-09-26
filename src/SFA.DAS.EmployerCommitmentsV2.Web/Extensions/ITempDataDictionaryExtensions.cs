using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class ITempDataDictionaryExtensions
    {
        public static void AddFlashMessage(this ITempDataDictionary tempData, string message, FlashMessageLevel level)
        {
            tempData["FlashMessage"] = message;
            tempData["FlashMessageLevel"] = level;
        }
    }
    public enum FlashMessageLevel
    {
        Info,
        Warning,
        Success
    }
}

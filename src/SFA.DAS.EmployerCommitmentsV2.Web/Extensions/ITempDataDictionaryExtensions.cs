using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class ITempDataDictionaryExtensions
    {
        public static readonly string FlashMessageTempDataKey = "FlashMessage";
        public static readonly string FlashMessageLevelTempDataKey = "FlashMessageLevel";

        public enum FlashMessageLevel
        {
            Info,
            Warning,
            Success
        }

        public static void AddFlashMessage(this ITempDataDictionary tempData, string message, FlashMessageLevel level)
        {
            tempData[FlashMessageTempDataKey] = message;
            tempData[FlashMessageLevelTempDataKey] = level;
        }
    }
}

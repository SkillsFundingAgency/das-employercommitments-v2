using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class ITempDataDictionaryExtensions
    {
        public static readonly string FlashMessageTempDataKey = "FlashMessage";
        public static readonly string FlashMessageSubTempDataKey = "FlashMessageSub";
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
            tempData[FlashMessageSubTempDataKey] = null;
            tempData[FlashMessageLevelTempDataKey] = level;
        }

        public static void AddFlashMessage(this ITempDataDictionary tempData, string message, string submessage, FlashMessageLevel level)
        {
            tempData[FlashMessageTempDataKey] = message;
            tempData[FlashMessageSubTempDataKey] = submessage;
            tempData[FlashMessageLevelTempDataKey] = level;
        }

        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o;
            tempData.TryGetValue(key, out o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }

        public static T GetButDontRemove<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            var result = Get<T>(tempData, key);
            if (result != null)
            {
                Put(tempData, key, result);
            }

            return result;
        }
    }
}

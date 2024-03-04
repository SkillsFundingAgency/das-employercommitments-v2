using Newtonsoft.Json;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

public static class CloneExtensions
{
    public static T ExplicitClone<T>(this T obj) where T : class, new()
    {
        return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
    }
}
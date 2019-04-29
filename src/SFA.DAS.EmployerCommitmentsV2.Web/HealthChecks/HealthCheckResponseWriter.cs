using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SFA.DAS.EmployerCommitmentsV2.Web.HealthChecks
{
    public static class HealthCheckResponseWriter
    {
        public static Task WriteJsonResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var jObject = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(e =>
                    new JProperty(e.Key, new JObject(
                        new JProperty("status", e.Value.Status.ToString()),
                        new JProperty("description", e.Value.Description),
                        new JProperty("data", new JObject(e.Value.Data.Select(d => 
                            new JProperty(d.Key, d.Value))))))))));

            var json = jObject.ToString(Formatting.Indented);
            
            return httpContext.Response.WriteAsync(json);
        }
    }
}
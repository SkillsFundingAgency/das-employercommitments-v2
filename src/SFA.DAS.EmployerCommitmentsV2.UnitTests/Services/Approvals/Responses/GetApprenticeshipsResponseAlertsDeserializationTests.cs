using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Types;

namespace SFA.DAS.EmployerCommitmentsV2.UnitTests.Services.Approvals.Responses;

public class GetApprenticeshipsResponseAlertsDeserializationTests
{
    [Test]
    public void Deserialize_ThenMapsIlrChangeInvalidAndChangesDeclinedFromApimNames()
    {
        const string json = """
            {
              "apprenticeships": [
                {
                  "alerts": [ "IlrChangeInvalid", "ChangesDeclined" ]
                }
              ]
            }
            """;

        var result = JsonConvert.DeserializeObject<GetApprenticeshipsResponse>(json, CamelCaseSettings());

        result.Apprenticeships.Should().ContainSingle();
        result.Apprenticeships.Single().Alerts.Should().Equal(Alerts.IlrChangeInvalid, Alerts.ChangesDeclined);
    }

    private static JsonSerializerSettings CamelCaseSettings()
    {
        return new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}

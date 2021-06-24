using System;
using SFA.DAS.Authorization.CommitmentPermissions.Client;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.EAS.Account.Api.Client;
using StructureMap;

namespace SFA.DAS.EmployerCommitmentsV2.Web.LocalDevRegistry
{
    public class LocalDevRegistry : Registry
    {
        public LocalDevRegistry()
        {
            var value = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (value == "Development")
            {
                For<ICommitmentsApiClientFactory>().ClearAll().Use<LocalDevApiClientFactory>();
                For<ICommitmentPermissionsApiClientFactory>().ClearAll().Use<LocalDevApiClientFactory>();
                For<IAccountApiClient>().ClearAll().Use<LocalAccountApiClient>();
            }
        }
    }
}

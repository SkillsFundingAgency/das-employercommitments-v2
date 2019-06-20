using SFA.DAS.Authorization.CommitmentPermissions;
using SFA.DAS.Authorization.CommitmentPermissions.Client;
using SFA.DAS.CommitmentsV2.Api.Client.Configuration;
using StructureMap;

namespace SFA.DAS.EmployerCommitmentsV2.Web.DependencyResolution
{
    public class CommitmentsPermissionsApiRegistry : Registry
    {
        public CommitmentsPermissionsApiRegistry()
        {
            IncludeRegistry<CommitmentPermissionsAuthorizationRegistry>();

            For<ICommitmentPermissionsApiClientFactory>().Use("", ctx =>
            {
                var config = ctx.GetInstance<CommitmentsClientApiConfiguration>();
                return new CommitmentPermissionsApiClientFactory(config);
            }).Singleton();
        }
    }
}
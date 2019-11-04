using SFA.DAS.CommitmentsV2.Api.Client.DependencyResolution;
using SFA.DAS.CommitmentsV2.Shared.DependencyInjection;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Services;
using StructureMap;

namespace SFA.DAS.EmployerCommitmentsV2.DependencyResolution.Shared
{
    public class _CommitmentsSharedRegistry : Registry
    {
        public _CommitmentsSharedRegistry()
        {
            Scan(
                scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("SFA.DAS.Commitments.Shared"));
                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                });

            For<ICommitmentsService>().Use<CommitmentsService>().Singleton();
            IncludeRegistry<ApprenticeshipInfoServiceRegistry>();
            IncludeRegistry<CommitmentsApiClientRegistry>();
            IncludeRegistry<EmployerAccountsRegistry>();
            IncludeRegistry<EncodingRegistry>();
        }
    }
}

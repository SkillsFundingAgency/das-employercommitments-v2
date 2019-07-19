using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.Commitments.Shared.Services;
using SFA.DAS.CommitmentsV2.Api.Client.DependencyResolution;
using StructureMap;

namespace SFA.DAS.Commitments.Shared.DependencyInjection
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
            IncludeRegistry<EncodingRegistry>();
        }
    }
}

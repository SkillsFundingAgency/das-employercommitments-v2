using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;

namespace SFA.DAS.EmployerCommitmentsV2.DependencyResolution
{
    public interface IApprovalsApiClientFactory
    {
        IApprovalsApiClient CreateClient();
    }
}

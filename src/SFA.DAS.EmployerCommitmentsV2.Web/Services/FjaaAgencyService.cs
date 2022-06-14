using System.Threading.Tasks;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Services
{
    public class FjaaAgencyService : IFjaaAgencyService
    {
        private readonly IApprovalsApiClient _approvalsApiClient;

        public FjaaAgencyService(IApprovalsApiClient approvalsApiClient)
        {
            _approvalsApiClient = approvalsApiClient;
        }

        public async Task<bool> AgencyExists(int legalEntityId)
        {
            GetAgencyResponse agency = null;

            if (legalEntityId > 0)
            {
                agency = await _approvalsApiClient.GetAgency(legalEntityId);

                if (agency != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
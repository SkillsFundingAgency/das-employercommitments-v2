using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Types;
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

        public async Task<List<DeliveryModel>> AssignDeliveryModels(List<DeliveryModel> models, bool agencyExists)
        {
            bool portable = models.Contains(DeliveryModel.PortableFlexiJob) ? true : false;

            if (agencyExists && !portable) { models.Remove(DeliveryModel.PortableFlexiJob); }
            if (agencyExists && portable) { models.Remove(DeliveryModel.PortableFlexiJob); }
            if (!agencyExists && portable) { models.Remove(DeliveryModel.FlexiJobAgency); }
            if (!agencyExists && !portable) { models.Remove(DeliveryModel.PortableFlexiJob); models.Remove(DeliveryModel.FlexiJobAgency); }

            return models;
        }
    }
}
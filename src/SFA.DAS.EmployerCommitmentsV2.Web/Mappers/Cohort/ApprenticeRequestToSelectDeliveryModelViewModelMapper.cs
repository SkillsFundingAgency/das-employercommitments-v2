using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;
using SFA.DAS.CommitmentsV2.Types;
using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class ApprenticeRequestToSelectDeliveryModelViewModelMapper : IMapper<ApprenticeRequest, SelectDeliveryModelViewModel>
    {
        private readonly IApprovalsApiClient _approvalsApiClient;
        private readonly IFjaaAgencyService _fjaaAgencyService;
        protected List<DeliveryModel> _deliveryModels;

        public ApprenticeRequestToSelectDeliveryModelViewModelMapper(IApprovalsApiClient approvalsApiClient, IFjaaAgencyService fjaaAgencyService)
            => (_approvalsApiClient, _fjaaAgencyService) = (approvalsApiClient, fjaaAgencyService);

        public async Task<SelectDeliveryModelViewModel> Map(ApprenticeRequest source)
        {
            var response = await _approvalsApiClient.GetProviderCourseDeliveryModels(source.ProviderId, source.CourseCode);

            _deliveryModels = response.DeliveryModels.ToList();

            bool agencyExists = await _fjaaAgencyService.AgencyExists((int)source.AccountLegalEntityId);

            bool portableAllowed = _deliveryModels.Contains(DeliveryModel.PortableFlexiJob) ? true : false;

            if (agencyExists && portableAllowed == false) { this.RemoveDeliveryModel((int)DeliveryModel.PortableFlexiJob); }
            if (!agencyExists && portableAllowed == true) { this.RemoveDeliveryModel((int)DeliveryModel.FlexiJobAgency); }

            return new SelectDeliveryModelViewModel
            { 
                AccountHashedId = source.AccountHashedId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                CourseCode = source.CourseCode,
                DeliveryModel = source.DeliveryModel,
                DeliveryModels = _deliveryModels.ToArray(),
                ProviderId = source.ProviderId,
                ReservationId = source.ReservationId,
                StartMonthYear = source.StartMonthYear,
                TransferSenderId = source.TransferSenderId
            };
        }

        protected void RemoveDeliveryModel(int deliveryModelId)
        {
            _deliveryModels.Remove((DeliveryModel)deliveryModelId);
        }

    }
}

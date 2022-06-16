using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;
using SFA.DAS.CommitmentsV2.Types;
using System.Collections.Generic;
using SFA.DAS.Authorization.Services;
using SFA.DAS.EmployerCommitmentsV2.Features;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class ApprenticeRequestToSelectDeliveryModelViewModelMapper : IMapper<ApprenticeRequest, SelectDeliveryModelViewModel>
    {
        private readonly IApprovalsApiClient _approvalsApiClient;
        private readonly IFjaaAgencyService _fjaaAgencyService;
        private readonly IAuthorizationService _authorizationService;
        protected List<DeliveryModel> _deliveryModels;

        public ApprenticeRequestToSelectDeliveryModelViewModelMapper(IApprovalsApiClient approvalsApiClient, IFjaaAgencyService fjaaAgencyService, IAuthorizationService authorizationService)
            => (_approvalsApiClient, _fjaaAgencyService, _authorizationService) = (approvalsApiClient, fjaaAgencyService, authorizationService);

        public async Task<SelectDeliveryModelViewModel> Map(ApprenticeRequest source)
        {
            var response = await _approvalsApiClient.GetProviderCourseDeliveryModels(source.ProviderId, source.CourseCode);
            _deliveryModels = response.DeliveryModels.ToList();

            if (_authorizationService.IsAuthorized(EmployerFeature.FJAA))
            {
                bool agencyExists = await _fjaaAgencyService.AgencyExists((int)source.AccountLegalEntityId);
                _deliveryModels = await _fjaaAgencyService.AssignDeliveryModels(_deliveryModels, agencyExists);
            }

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

using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class EditApprenticeshipRequestViewModelToEditApprenticeshipDeliveryModelViewModelMapper : IMapper<EditApprenticeshipRequestViewModel, EditApprenticeshipDeliveryModelViewModel>
{
    private readonly IApprovalsApiClient _approvalsApiClient;

    public EditApprenticeshipRequestViewModelToEditApprenticeshipDeliveryModelViewModelMapper(IApprovalsApiClient approvalsApiClient)
    {
        _approvalsApiClient = approvalsApiClient;
    }

    public async Task<EditApprenticeshipDeliveryModelViewModel> Map(EditApprenticeshipRequestViewModel source)
    {
        var response = await _approvalsApiClient.GetEditApprenticeshipDeliveryModel(source.AccountId, source.ApprenticeshipId);

        return new EditApprenticeshipDeliveryModelViewModel
        {
            LegalEntityName = response.LegalEntityName,
            DeliveryModel = (DeliveryModel) source.DeliveryModel,
            DeliveryModels = response.DeliveryModels
        };
    }
}
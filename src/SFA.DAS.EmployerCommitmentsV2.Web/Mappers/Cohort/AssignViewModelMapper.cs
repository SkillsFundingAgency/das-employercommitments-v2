using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class AssignViewModelMapper : IMapper<AssignRequest, AssignViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;

    public AssignViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
    {
        _commitmentsApiClient = commitmentsApiClient;
    }

    public async Task<AssignViewModel> Map(AssignRequest request)
    {
        var accountLegalEntity = await _commitmentsApiClient.GetAccountLegalEntity(request.AccountLegalEntityId);

        return new AssignViewModel
        {
            AccountHashedId = request.AccountHashedId,
            AccountLegalEntityHashedId = request.AccountLegalEntityHashedId,
            LegalEntityName = accountLegalEntity.LegalEntityName,
            ReservationId = request.ReservationId,
            StartMonthYear = request.StartMonthYear,
            CourseCode = request.CourseCode,
            ProviderId = request.ProviderId,
            TransferSenderId = request.TransferSenderId,
            EncodedPledgeApplicationId = request.EncodedPledgeApplicationId
        };
    }
}
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class IndexViewModelMapper(ICommitmentsApiClient client) : IMapper<IndexRequest, IndexViewModel>
{
    public async Task<IndexViewModel> Map(IndexRequest source)
    {
        var account = await client.GetAccount(source.AccountId);
        return new IndexViewModel
        {
            AccountHashedId = source.AccountHashedId,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            ReservationId = source.ReservationId,
            StartMonthYear = source.StartMonthYear,
            CourseCode = source.CourseCode,
            Origin = source.ReservationId.HasValue ? Origin.Reservations : Origin.Apprentices,
            IsLevyFunded = account.LevyStatus == ApprenticeshipEmployerType.Levy
        };
    }
}
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class PauseRequestToViewModelMapper : IMapper<PauseRequest, PauseRequestViewModel>
    {
        private readonly ICommitmentsApiClient _client;
        public PauseRequestToViewModelMapper(ICommitmentsApiClient client)
        {
            _client = client;
        }

        public async Task<PauseRequestViewModel> Map(PauseRequest source)
        {
            var apprenticeship = await _client.GetApprenticeship(source.ApprenticeshipId);

            return new PauseRequestViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ApprenticeName = $"{apprenticeship.FirstName} {apprenticeship.LastName}",
                ULN = apprenticeship.Uln,
                Course = apprenticeship.CourseName,
                PauseDate = DateTime.UtcNow
            };
        }
    }
}

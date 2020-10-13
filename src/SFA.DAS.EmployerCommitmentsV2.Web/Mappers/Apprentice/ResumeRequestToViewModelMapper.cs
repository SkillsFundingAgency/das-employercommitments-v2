using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ResumeRequestToViewModelMapper : IMapper<ResumeRequest, ResumeRequestViewModel>
    {
        private readonly ICommitmentsApiClient _client;
        public ResumeRequestToViewModelMapper(ICommitmentsApiClient client)
        {
            _client = client;
        }

        public async Task<ResumeRequestViewModel> Map(ResumeRequest source)
        {
            var apprenticeship = await _client.GetApprenticeship(source.ApprenticeshipId);

            return new ResumeRequestViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ApprenticeName = $"{apprenticeship.FirstName} {apprenticeship.LastName}",
                ULN = apprenticeship.Uln,
                Course = apprenticeship.CourseName,
                PauseDate = apprenticeship.PauseDate,
                ResumeDate = DateTime.UtcNow
            };
        }
    }
}

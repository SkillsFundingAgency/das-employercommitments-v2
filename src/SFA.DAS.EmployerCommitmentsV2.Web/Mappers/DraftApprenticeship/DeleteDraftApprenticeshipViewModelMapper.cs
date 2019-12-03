using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class DeleteDraftApprenticeshipViewModelMapper : IMapper<DeleteApprenticeshipRequest, DeleteDraftApprenticeshipViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly ILinkGenerator _linkGenerator;

        public DeleteDraftApprenticeshipViewModelMapper(ICommitmentsApiClient commitmentsApiClient, ILinkGenerator linkGenerator)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _linkGenerator = linkGenerator;
        }

        public async Task<DeleteDraftApprenticeshipViewModel> Map(DeleteApprenticeshipRequest source)
        {
            async Task<(string FirstName, string LastName, bool IsLastApprenticeship)> GetDraftApprenticeship()
            {
                var draftApprenticeships = (await _commitmentsApiClient.GetDraftApprenticeships(source.CohortId)).DraftApprenticeships;
                var draftApprenticeship = draftApprenticeships.FirstOrDefault(x => x.Id == source.DraftApprenticeshipId);
                if (draftApprenticeship == null)
                {
                    throw new DraftApprenticeshipNotFoundException(
                        $"Apprenticeship Id: {source.DraftApprenticeshipId} not found");
                }

                return (draftApprenticeship.FirstName, draftApprenticeship.LastName, draftApprenticeships.Count == 1);
            }

            var cohort = await _commitmentsApiClient.GetCohort(source.CohortId);
            if (cohort.WithParty != Party.Employer)
            {
                throw new CohortEmployerUpdateDeniedException($"Cohort {cohort.CohortId} is not With the Employer");
            }
            var apprenticeshipToDelete = await GetDraftApprenticeship();

            return new DeleteDraftApprenticeshipViewModel()
            {
                FirstName = apprenticeshipToDelete.FirstName,
                LastName = apprenticeshipToDelete.LastName,
                IsLastApprenticeshipInCohort = apprenticeshipToDelete.IsLastApprenticeship,
                AccountHashedId = source.AccountHashedId,
                DraftApprenticeshipHashedId = source.DraftApprenticeshipHashedId,
                CohortReference = source.CohortReference,
                Origin = source.Origin
            };
        }
    }
}

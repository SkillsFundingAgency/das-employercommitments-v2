using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
            var cohort = await _commitmentsApiClient.GetCohort(source.CohortId);


            if (cohort.WithParty != Party.Employer)
            {
                throw new CohortEmployerUpdateDeniedException($"Cohort {cohort.CohortId} is not With the Employer");
            }

            var draftApprenticeship = await _commitmentsApiClient.GetDraftApprenticeship(source.CohortId, source.DraftApprenticeshipId);

            var backOrCancelButtonUrl = (source.Origin == Enums.Origin.CohortDetails) 
                ? _linkGenerator.CommitmentsV2Link($"{source.AccountHashedId}/unapproved/{source.CohortReference}")
                : _linkGenerator.CommitmentsV2Link($"{source.AccountHashedId}/unapproved/{source.CohortReference}/apprentices/{source.DraftApprenticeshipHashedId}/edit");

            return new DeleteDraftApprenticeshipViewModel()
            {
                FirstName = draftApprenticeship.FirstName,
                LastName = draftApprenticeship.LastName,
                AccountHashedId = source.AccountHashedId,
                DraftApprenticeshipHashedId = source.DraftApprenticeshipHashedId,
                CohortReference = source.CohortReference,
                Origin = source.Origin,
                RedirectToOriginUrl = backOrCancelButtonUrl
            };
        }
    }
}

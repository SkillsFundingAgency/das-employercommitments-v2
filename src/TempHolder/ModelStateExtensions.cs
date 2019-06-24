using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;

namespace SFA.DAS.Commitments.Shared.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddModelExceptionErrors(this ModelStateDictionary modelState, CommitmentsApiModelException exception, Func<string, string> fieldNameMapper = null)
        {
            if (exception?.Errors == null)
            {
                return;
            }

            foreach (var error in exception.Errors)
            {
                var field = fieldNameMapper == null ? error.Field : fieldNameMapper(error.Field);

                modelState.AddModelError(field, error.Message);
            }
        }
    }

    namespace SFA.DAS.Commitments.Shared.Interfaces
    {

        public interface ICommitmentsService
        {
            Task<CohortDetails> GetCohortDetail(long cohortId);
            Task AddDraftApprenticeshipToCohort(long cohortId, AddDraftApprenticeshipRequest request);

            Task<EditDraftApprenticeshipDetails> GetDraftApprenticeshipForCohort(long cohortId,
                long draftApprenticeshipId);

            Task UpdateDraftApprenticeship(long cohortId, long draftApprenticeshipId,
                UpdateDraftApprenticeshipRequest updateRequest);
        }

    }

    namespace SFA.DAS.Commitments.Shared.Models
    {
        public class CohortDetails
    {
        public long CohortId { get; set; }
        public string HashedCohortId { get; set; }
        public string LegalEntityName { get; set; }
    }

        public class EditDraftApprenticeshipDetails
        {
            public long DraftApprenticeshipId { get; set; }
            public string DraftApprenticeshipHashedId { get; set; }
            public long CohortId { get; set; }
            public int ProviderId { get; set; }
            public string CohortReference { get; set; }
            public Guid? ReservationId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string UniqueLearnerNumber { get; set; }
            public string CourseCode { get; set; }
            public int? Cost { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string OriginatorReference { get; set; }
        }

    }
}
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers
{
    public class ConfirmProviderAssignRequestMapper : IMapper<ConfirmProviderViewModel, AssignRequest>
    {
        public AssignRequest Map(ConfirmProviderViewModel source)
        {
            return new AssignRequest
            {
                AccountHashedId = source.AccountHashedId,
                CourseCode = source.CourseCode,
                ReservationId = source.ReservationId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                StartMonthYear = source.StartMonthYear,
                ProviderId = source.ProviderId
            };
        }
    }
}
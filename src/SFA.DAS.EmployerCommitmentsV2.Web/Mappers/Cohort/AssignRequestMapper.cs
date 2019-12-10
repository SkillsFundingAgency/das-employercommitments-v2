using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class AssignRequestMapper : IMapper<ConfirmProviderViewModel, AssignRequest>
    {
        public Task<AssignRequest> Map(ConfirmProviderViewModel source)
        {
            return Task.FromResult(new AssignRequest
            {
                AccountHashedId = source.AccountHashedId,
                CourseCode = source.CourseCode,
                ReservationId = source.ReservationId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                StartMonthYear = source.StartMonthYear,
                ProviderId = source.ProviderId,
                TransferSenderId = source.TransferSenderId
            });
        }
    }
}
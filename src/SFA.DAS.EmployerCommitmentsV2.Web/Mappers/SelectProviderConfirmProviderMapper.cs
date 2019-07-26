using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers
{
    public class SelectProviderConfirmProviderMapper : IMapper<ConfirmProviderViewModel, SelectProviderViewModel>
    {
        public SelectProviderViewModel Map(ConfirmProviderViewModel source)
        {
            return new SelectProviderViewModel
            {
                ProviderId = source.ProviderId.ToString(),
                ReservationId = source.ReservationId,
                CourseCode = source.CourseCode,
                AccountHashedId = source.AccountHashedId,
                EmployerAccountLegalEntityPublicHashedId = source.EmployerAccountLegalEntityPublicHashedId,
                StartMonthYear = source.StartMonthYear
            };
        }
    }
}
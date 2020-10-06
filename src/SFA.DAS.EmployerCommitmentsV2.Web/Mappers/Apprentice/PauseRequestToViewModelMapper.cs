using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class PauseRequestToViewModelMapper : IMapper<PauseRequest, PauseRequestViewModel>
    {
        public PauseRequestToViewModelMapper()
        {
            // Inject commitment api
            // Inject Fat api
        }

        public Task<PauseRequestViewModel> Map(PauseRequest source)
        {
            /// Get details of apprentice from commitment api
            /// Get course details from fat api
            /// 
            var result = new PauseRequestViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ApprenticeName = "Demo User",
                ULN = "6744927239",
                Course = "Able seafarer (deck)",
                PauseDate = DateTime.UtcNow
            };

            return Task.FromResult(result);
        }
    }
}

using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class SelectDeliveryModelViewModelToApprenticeRequestMapper : IMapper<SelectDeliveryModelViewModel, ApprenticeRequest>
    {
        public Task<ApprenticeRequest> Map(SelectDeliveryModelViewModel source)
        {
            return Task.FromResult(new ApprenticeRequest
            {
                AccountHashedId = source.AccountHashedId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                CourseCode = source.CourseCode,
                DeliveryModel = source.DeliveryModel,
                ProviderId = source.ProviderId,
                ReservationId = source.ReservationId,
                StartMonthYear = source.StartMonthYear,
                TransferSenderId = source.TransferSenderId,
                EncodedPledgeApplicationId = source.EncodedPledgeApplicationId,
                ShowTrainingDetails = source.ShowTrainingDetails
            });
        }
    }
}

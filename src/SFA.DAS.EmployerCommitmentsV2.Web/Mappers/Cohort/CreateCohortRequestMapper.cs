using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class CreateCohortRequestMapper : 
        IMapper<ApprenticeViewModel, CreateCohortRequest>
    {
        public Task<CreateCohortRequest> Map(ApprenticeViewModel source)
        {
            return Task.FromResult(new CreateCohortRequest
            {
                AccountId = source.AccountId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                ProviderId = source.ProviderId,
                ReservationId = source.ReservationId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth.Date,
                Uln = source.Uln,
                DeliveryModel = source.DeliveryModel,
                CourseCode = source.CourseCode,
                Cost = source.Cost,
                StartDate = source.StartDate.Date,
                EndDate = source.EndDate.Date,
                OriginatorReference = source.Reference,
                TransferSenderId = source.DecodedTransferSenderId,
                PledgeApplicationId = (int?)source.PledgeApplicationId
            });
        }
     }
}

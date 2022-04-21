using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ConfirmEditApprenticehsipViewModelToEditApiRequestMapper : IMapper<ConfirmEditApprenticeshipViewModel, EditApprenticeshipApiRequest>
    {
        public Task<EditApprenticeshipApiRequest> Map(ConfirmEditApprenticeshipViewModel source)
        {
            return Task.FromResult(new EditApprenticeshipApiRequest
            {
                ApprenticeshipId = source.ApprenticeshipId,
                AccountId = source.AccountId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth,
                Cost = source.Cost,
                EmployerReference = source.EmployerReference,
                StartDate = source.StartDate,
                EndDate = source.EndDate,
                DeliveryModel = source.DeliveryModel,
                EmploymentEndDate = source.EmploymentEndDate,
                EmploymentPrice = source.EmploymentPrice,
                CourseCode = source.CourseCode,
                Version = source.Version,
                Option = source.Option == "TBC" ? string.Empty : source.Option
            });
        }
    }
}

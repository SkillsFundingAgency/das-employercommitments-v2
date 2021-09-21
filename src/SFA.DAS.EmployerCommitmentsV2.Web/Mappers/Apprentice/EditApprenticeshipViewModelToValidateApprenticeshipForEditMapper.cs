using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;


namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class EditApprenticeshipViewModelToValidateApprenticeshipForEditMapper : IMapper<EditApprenticeshipRequestViewModel, ValidateApprenticeshipForEditRequest>
    {
        private readonly IAuthenticationService _authenticationService;

        public EditApprenticeshipViewModelToValidateApprenticeshipForEditMapper(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public Task<ValidateApprenticeshipForEditRequest> Map(EditApprenticeshipRequestViewModel source)
        {
            var result = new ValidateApprenticeshipForEditRequest
            {
                EmployerAccountId = source.AccountId,
                ApprenticeshipId = source.ApprenticeshipId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth.Date,
                ULN = source.ULN,
                Cost = source.Cost,
                EmployerReference = source.EmployerReference,
                StartDate = source.StartDate.Date,
                EndDate = source.EndDate.Date,
                TrainingCode = source.CourseCode,
                Version = source.Version,
                Option = source.Option
            };
            return Task.FromResult(result);
        }
    }
}

using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ConfirmEditApprenticeshipViewModelToConfirmEditApprenticeshipRequestMapper(
    IAuthenticationService authenticationService)
    : IMapper<ConfirmEditApprenticeshipViewModel,
        SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests.ConfirmEditApprenticeshipRequest>
{
    public Task<SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests.ConfirmEditApprenticeshipRequest> Map(ConfirmEditApprenticeshipViewModel source)
    {
        return Task.FromResult(new SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests.ConfirmEditApprenticeshipRequest
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
            DeliveryModel = source.DeliveryModel?.ToString(),
            EmploymentEndDate = source.EmploymentEndDate,
            EmploymentPrice = source.EmploymentPrice,
            CourseCode = source.CourseCode,
            Version = source.Version,
            Option = source.Option == "TBC" ? string.Empty : source.Option,
            UserInfo = new ApimUserInfo
            {
                UserDisplayName = authenticationService.UserName,
                UserEmail = authenticationService.UserEmail,
                UserId = authenticationService.UserId
            }
        });
    }
} 
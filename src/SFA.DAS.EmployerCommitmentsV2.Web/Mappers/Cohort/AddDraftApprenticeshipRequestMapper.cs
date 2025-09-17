using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class AddDraftApprenticeshipRequestMapper : IMapper<AddDraftApprenticeshipViewModel, AddDraftApprenticeshipApimRequest>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<AddDraftApprenticeshipRequestMapper> _logger;

    public AddDraftApprenticeshipRequestMapper(IAuthenticationService authenticationService, ILogger<AddDraftApprenticeshipRequestMapper> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    public Task<AddDraftApprenticeshipApimRequest> Map(AddDraftApprenticeshipViewModel source)
    {
        _logger.LogInformation("Mapping AddDraftApprenticeshipViewModel to AddDraftApprenticeshipApimRequest : Course Code = {0}", source.CourseCode);

        return Task.FromResult(new AddDraftApprenticeshipApimRequest
        {
            UserId = _authenticationService.UserId,
            ProviderId = 1, // TODO: Remove this from the request as it's not required
            FirstName = source.FirstName,
            LastName = source.LastName,
            DateOfBirth = source.DateOfBirth.Date,
            Email = source.Email, 
            Uln = source.Uln,
            DeliveryModel = source.DeliveryModel,
            CourseCode = source.CourseCode,
            Cost = source.Cost,
            EmploymentPrice = source.EmploymentPrice,
            StartDate = source.StartDate.Date,
            EndDate = source.EndDate.Date,
            EmploymentEndDate = source.EmploymentEndDate.Date,
            OriginatorReference = source.Reference,
            ReservationId = source.ReservationId,
            ActualStartDate = source.ActualStartDate,
            UserInfo = new ApimUserInfo
            {
                UserDisplayName = _authenticationService.UserName,
                UserEmail = _authenticationService.UserEmail,
                UserId = _authenticationService.UserId
            }
        });
    }
}
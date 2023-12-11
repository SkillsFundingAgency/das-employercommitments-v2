using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class EditEndDateRequestMapper : IMapper<EditEndDateViewModel, CommitmentsV2.Api.Types.Requests.EditEndDateRequest>
{
    private readonly IAuthenticationService _authenticationService;

    public EditEndDateRequestMapper(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    
    public Task<CommitmentsV2.Api.Types.Requests.EditEndDateRequest> Map(EditEndDateViewModel source)
    {
        var result = new CommitmentsV2.Api.Types.Requests.EditEndDateRequest
        {
            ApprenticeshipId = source.ApprenticeshipId,
            EndDate = source.EndDate.Date,
            UserInfo = _authenticationService.UserInfo
        };

        return Task.FromResult(result);
    }
}
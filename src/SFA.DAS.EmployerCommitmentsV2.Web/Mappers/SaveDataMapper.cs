using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers
{
    public abstract class SaveDataMapper<T> where T : SaveDataRequest
    {
        private readonly IAuthenticationService _authenticationService;

        public SaveDataMapper(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        protected UserInfo GetUserInfo()
        {
            if (_authenticationService.IsUserAuthenticated())
            {
                return new UserInfo
                {
                    UserId = _authenticationService.UserId,
                    UserDisplayName = _authenticationService.UserName,
                    UserEmail = _authenticationService.UserEmail
                };
            }

            return null;
        }
    }
}

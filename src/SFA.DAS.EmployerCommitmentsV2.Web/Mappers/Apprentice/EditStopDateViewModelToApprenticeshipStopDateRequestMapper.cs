using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class EditStopDateViewModelToApprenticeshipStopDateRequestMapper : IMapper<EditStopDateViewModel, ApprenticeshipStopDateRequest>
    {
        private readonly IAuthenticationService _authenticationService;        

        public EditStopDateViewModelToApprenticeshipStopDateRequestMapper(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;            
        }

        public Task<ApprenticeshipStopDateRequest> Map(EditStopDateViewModel source)
        {
            var result = new ApprenticeshipStopDateRequest()
            {
                AccountId = source.AccountId,
                NewStopDate = new DateTime(source.NewStopDate.Year.Value, source.NewStopDate.Month.Value, source.NewStopDate.Day.Value),
                UserInfo = _authenticationService.UserInfo
            };

            return Task.FromResult(result);
        }
    }
}

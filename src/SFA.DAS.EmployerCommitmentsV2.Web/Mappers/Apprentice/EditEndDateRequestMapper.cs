using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class EditEndDateRequestMapper : IMapper<EditEndDateViewModel, UpdateEndDateOfCompletedRecordRequest>
    {
        private readonly IAuthenticationService _authenticationService;

        public EditEndDateRequestMapper(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public Task<UpdateEndDateOfCompletedRecordRequest> Map(EditEndDateViewModel source)
        {
            var result = new UpdateEndDateOfCompletedRecordRequest
            {
               ApprenticeshipId = source.ApprenticeshipId,
               EndDate = source.EndDate.Date,
               UserInfo = _authenticationService.UserInfo
            };

            return Task.FromResult(result);
        }
    }
}

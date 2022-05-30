using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Agency;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dasync.Collections;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Services
{
    public class FjaaAgencyService : IFjaaAgencyService
    {
        private readonly IApprovalsApiClient _approvalsApiClient;

        public FjaaAgencyService(IApprovalsApiClient approvalsApiClient)
        {
            _approvalsApiClient = approvalsApiClient;
        }

        public async Task<bool> AgencyExists(int legalIdentityId)
        {
            GetAgencyResponse agency = null;

            if (legalIdentityId > 0)
            {
                agency = await _approvalsApiClient.GetAgency(legalIdentityId);

                if (agency != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
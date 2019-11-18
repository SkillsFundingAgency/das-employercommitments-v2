using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class DraftRequest : IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }
        public long AccountId { get; set; }

    }
}

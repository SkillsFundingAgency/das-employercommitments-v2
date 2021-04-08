using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class CommitmentInformViewModel : IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }
    }
}

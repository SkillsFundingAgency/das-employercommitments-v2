using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.EmployerCommitmentsV2.Web.Enums;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship
{
    public class DeleteDraftApprenticeshipRequest : IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }
        public string CohortReference { get; set; }
        public long CohortId { get; set; }
        public long AccountId { get; set; }
        public long DraftApprenticeshipId { get; set; }
        public string DraftApprenticeshipHashedId { get; set; }
        public string Origin { get; set; }
    }
}

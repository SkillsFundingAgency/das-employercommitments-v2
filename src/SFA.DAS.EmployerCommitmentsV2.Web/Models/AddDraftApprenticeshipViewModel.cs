using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Authorization;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models
{
    public class AddDraftApprenticeshipViewModel : DraftApprenticeshipViewModel, IAuthorizationContextModel
    {
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }

        public long? AccountLegalEntityId { get; set; }
    }
}

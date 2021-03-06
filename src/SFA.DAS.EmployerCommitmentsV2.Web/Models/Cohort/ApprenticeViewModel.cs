﻿using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class ApprenticeViewModel : DraftApprenticeshipViewModel, IAuthorizationContextModel
    {
        public long AccountId { get; set; }
        public string AccountHashedId { get; set; }

        public long AccountLegalEntityId { get; set; }
        public string AccountLegalEntityHashedId { get; set; }
        public string LegalEntityName { get; set; }
        public string TransferSenderId { get; set; }
        public long? DecodedTransferSenderId { get; set; }
        public Origin Origin { get; set; }
        public bool AutoCreatedReservation { get; set; }
    }

}

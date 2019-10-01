﻿using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class DetailsViewModel : IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }
        public Party WithParty { get; set; }
        public string CohortReference { get; set; }
        public long CohortId { get; set; }
        public string AccountLegalEntityHashedId { get; set; }
        public string LegalEntityName { get; set; }
        public string ProviderName { get; set; }
        public string Message { get; set; }
        public string TransferSenderHashedId { get; set; }

        public int DraftApprenticeshipsCount => Courses?.SelectMany(c => c.DraftApprenticeships).Count() ?? 0;
        
        public IReadOnlyCollection<DetailsViewCourseGroupingModel> Courses { get; set; }
        public string PageTitle { get; set; }
        public CohortDetailsOptions? Selection { get; set; }
        public string SendMessage { get; set; }
        public string ApproveMessage { get; set; }
        public bool IsApprovedByProvider { get; set; }
    }
    public enum CohortDetailsOptions
    {
        Send,
        Approve
    }
}

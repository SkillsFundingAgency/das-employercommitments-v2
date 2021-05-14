﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit
{
    public class ReviewApprenticeshipUpdatesRequestViewModel : IViewReviewApprenticeshipUpdatesViewModel, IAuthorizationContextModel
    {
        public bool? ApproveChanges { get; set; }

        public string ProviderName { get; set; }

        public BaseEdit OriginalApprenticeship { get; set; }
        [FromRoute]
        public string AccountHashedId { get; set; }
        [JsonIgnore]
        public long AccountId { get; set; }
        [FromRoute]
        public string ApprenticeshipHashedId { get; set; }
        [JsonIgnore]
        public long ApprenticeshipId { get; set; }
        public BaseEdit ApprenticeshipUpdates { get; set; }
    }
}

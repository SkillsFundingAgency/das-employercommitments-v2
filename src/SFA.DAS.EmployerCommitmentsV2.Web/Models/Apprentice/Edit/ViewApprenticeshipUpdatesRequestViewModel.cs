﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit
{
    public class ViewApprenticeshipUpdatesRequestViewModel : IViewReviewApprenticeshipUpdatesViewModel , IAuthorizationContextModel
    {
        public ViewApprenticeshipUpdatesRequestViewModel()
        {
            ApprenticeshipUpdates = new BaseEdit();
            OriginalApprenticeship = new BaseEdit();
        }
        public BaseEdit ApprenticeshipUpdates { get; set; }
        public bool? UndoChanges { get; set; }
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
    }

   
}

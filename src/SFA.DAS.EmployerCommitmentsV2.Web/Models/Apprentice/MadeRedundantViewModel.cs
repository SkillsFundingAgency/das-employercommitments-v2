﻿using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class MadeRedundantViewModel : IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }

        public string ApprenticeshipHashedId { get; set; }

        public long ApprenticeshipId { get; set; }

        public int? StopMonth { get; set ; }
        
        public int? StopYear { get; set; }

        public bool IsCoPJourney { get; set; }

        public bool? MadeRedundant { get; set; }

        public string ApprenticeName { get; set; }
    }
}
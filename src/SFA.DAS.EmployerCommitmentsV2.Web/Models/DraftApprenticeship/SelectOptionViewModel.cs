﻿using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship
{
    public class SelectOptionViewModel : DraftApprenticeshipViewModel, IAuthorizationContextModel
    {
        public SelectOptionViewModel(DateTime? dateOfBirth, DateTime? startDate, DateTime? endDate) : base(dateOfBirth, startDate, endDate)
        {
        }

        public SelectOptionViewModel()
        {
        }

        public string AccountHashedId { get; set; }
        public long DraftApprenticeshipId { get; set; }

        public string StandardTitle { get; set; }
        public string Version { get; set; }
        public IEnumerable<string> Options { get; set; }
    }
}

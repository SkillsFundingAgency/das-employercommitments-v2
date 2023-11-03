using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class ViewEmployerAgreementModel :  IAuthorizationContextModel
    {
        [FromRoute]
        public string AccountHashedId { get; set; }

        public long CohortId { get; set; }
    }
}

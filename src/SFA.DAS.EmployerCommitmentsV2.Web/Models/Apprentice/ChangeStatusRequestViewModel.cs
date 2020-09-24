using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ChangeStatusRequestViewModel : IAuthorizationContextModel
    {
        public ChangeStatusRequestViewModel()
        {
        }

        public string AccountHashedId { get; set; }

        public string ApprenticeshipHashedId { get; set; }

        public long ApprenticeshipId { get; set; }

        public ChangeStatusType? SelectedStatusChange { get; set; }
    }

    public enum ChangeStatusType
    {
        Stop,
        Pause,
        GoBack
    }
}

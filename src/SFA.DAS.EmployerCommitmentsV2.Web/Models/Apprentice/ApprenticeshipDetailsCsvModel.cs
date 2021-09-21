using System;
using System.Collections.Generic;
using System.Globalization;
using CsvHelper.Configuration.Attributes;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Extensions;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ApprenticeshipDetailsCsvModel
    {
        [Name("Apprentice name")]
        public string ApprenticeName { get ; private set ; }
        [Name("Apprenticeship training course")]
        public string CourseName { get; private set; }
        [Name("Planned start date")]
        public string PlannedStartDate { get; private set; }
        [Name("Planned end date")]
        public string PlannedEndDate { get ; private set ; }
        [Name("Paused date")]
        public string PausedDate { get; private set; }
        [Name("Training provider")]
        public string Provider { get; private set; }
        [Name("Reference")]
        public string CohortReference { get; private set; }
        [Name("ULN")]
        public string Uln { get; private set; }
        [Name("Date of birth")]
        public string DateOfBirth { get; private set; }
        [Name("Total agreed apprenticeship price")]
        public string TotalAgreedPrice { get; private set; }
        [Name("Your reference")]
        public string EmployerRef { get; private set; }
        [Name("Apprentice confirmation")]
        public string ApprenticeConfirmation { get; private set; }
        [Name("Status")]
        public string Status { get ; private set ; }
        [Name("Alerts")]
        public string Alerts { get ; private set ; }

        public static implicit operator ApprenticeshipDetailsCsvModel(GetApprenticeshipsResponse.ApprenticeshipDetailsResponse model)
        {
            return new ApprenticeshipDetailsCsvModel
            {
                ApprenticeName = $"{model.FirstName} {model.LastName}",
                Provider = model.ProviderName,
                CourseName = model.CourseName,
                PlannedStartDate = model.StartDate.ToGdsFormatWithoutDay(),
                PlannedEndDate = model.EndDate.ToGdsFormatWithoutDay(),
                PausedDate = model.PauseDate != DateTime.MinValue ?  model.PauseDate.ToGdsFormatWithoutDay() : "",
                CohortReference = model.CohortReference,
                EmployerRef = model.EmployerRef,
                Uln = model.Uln,
                DateOfBirth = model.DateOfBirth.ToGdsFormat(),
                TotalAgreedPrice = $"{model.TotalAgreedPrice.Value as object:n0}",
                ApprenticeConfirmation = model.ConfirmationStatus?.GetDescription(),
                Status = model.ApprenticeshipStatus.GetDescription(),
                Alerts = GenerateAlerts(model.Alerts)
            };
        }

        private static string GenerateAlerts(IEnumerable<Alerts> alerts)
        {
            var alertString = string.Empty;

            foreach (var alert in alerts)
            {
                if (!string.IsNullOrWhiteSpace(alertString))
                {
                    alertString += "|";
                }
                alertString += alert.GetDescription();
            }

            return alertString;
        }
    }
}
using System;
using SFA.DAS.CommitmentsV2.Shared.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class CohortDraftApprenticeshipViewModel
    {
        public long Id { get; set; }
        public string DraftApprenticeshipHashedId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Cost { get; set; }
        public int? FundingBandCap { get; set; }
        public string ULN { get; set; }
        public bool IsComplete { get; set; }

        public bool ExceedsFundingBandCap
        {
            get
            {
                if (Cost.HasValue && FundingBandCap.HasValue)
                {
                    return Cost.Value > FundingBandCap.Value;
                }

                return false;
            }
        }

        public string DisplayName => $"{FirstName} {LastName}";

        public string DisplayDateOfBirth => DateOfBirth?.ToGdsFormat() ?? "-";

        public string DisplayTrainingDates => ToGdsFormatDateRange(StartDate, EndDate);

        public string DisplayEmploymentDates => ToGdsFormatDateRange(StartDate, EmploymentEndDate);

        public string DisplayCost => Cost?.ToGdsCostFormat() ?? "-";
        public string DisplayEmploymentPrice => EmploymentPrice?.ToGdsCostFormat() ?? "-";

        public DateTime? OriginalStartDate { get; set; }

        public bool HasOverlappingUln { get; set; }
        public bool HasOverlappingEmail { get; set; }
        public int? EmploymentPrice { get; set; }
        public DateTime? EmploymentEndDate { get; set; }

        private string ToGdsFormatDateRange(DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue)
                return $"{start.Value.ToGdsFormatWithoutDay()} to {end.Value.ToGdsFormatWithoutDay()}";
            else
                return "-";
        }
    }
}

using System;
using SFA.DAS.Commitments.Shared.Extensions;

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
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int? Cost { get; set; }

        public string DisplayName => $"{FirstName} {LastName}";

        public string DisplayDateOfBirth => DateOfBirth.HasValue ? DateOfBirth.Value.ToGdsFormat() : "-";

        public string DisplayTrainingDates
        {
            get
            {
                if (StartDate.HasValue && EndDate.HasValue)
                {
                    return $"{StartDate.Value.ToGdsFormatWithoutDay()} to {EndDate.Value.ToGdsFormatWithoutDay()}";
                }

                return "-";
            }
        }

        public string DisplayCost => Cost.HasValue ? $"{Cost.Value.ToGdsCostFormat()}" : "-";
    }
}

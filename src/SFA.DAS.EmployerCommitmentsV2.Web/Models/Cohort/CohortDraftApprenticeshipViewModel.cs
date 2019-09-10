using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class CohortDraftApprenticeshipViewModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int? Cost { get; set; }

        public string DisplayName => $"{FirstName} {LastName}";

        public string DisplayDateOfBirth => DateOfBirth.HasValue ? $"{DateOfBirth.Value:d MMM yyyy}" : "-";

        public string DisplayTrainingDates
        {
            get
            {
                if (StartDate.HasValue && EndDate.HasValue)
                {
                    return $"{StartDate.Value:MMM yyyy} to {EndDate.Value:MMM yyyy}";
                }

                return "-";
            }
        }

        public string DisplayCost => Cost.HasValue ? $"£{Cost.Value:n0}" : "-";
    }
}

using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class EditApprenticeshipRequestViewModel : IAuthorizationContextModel
    {
        public EditApprenticeshipRequestViewModel()
        {
            DateOfBirth = new DateModel();
            StartDate = new MonthYearModel("");
            EndDate = new MonthYearModel("");
        }

        public EditApprenticeshipRequestViewModel(DateTime? dateOfBirth, DateTime? startDate, DateTime? endDate) : base()
        {
            DateOfBirth = dateOfBirth == null ? new DateModel() : new DateModel(dateOfBirth.Value);
            StartDate = startDate == null ? new MonthYearModel("") : new MonthYearModel($"{startDate.Value.Month}{startDate.Value.Year}");
            EndDate = endDate == null ? new MonthYearModel("") : new MonthYearModel($"{endDate.Value.Month}{endDate.Value.Year}");
        }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string HashedApprenticeshipId { get; set; }
        public long ApprenticeshipId { get; set; }
        public string AccountHashedId { get; set; }
        public long AccountId { get; set; }
        public string ApprenticeName { get; set; }       
        public string ULN { get; set; }

        public DateTime? StopDate { get; set; }
        public DateTime? PauseDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public ProgrammeType? TrainingType { get; set; }
        public string TrainingName { get; set; }

        [Display(Name = "Total agreed apprenticeship price (excluding VAT)")]
        [SuppressArgumentException(nameof(Cost), "The apprenticeship price is not valid")]
        public decimal? Cost { get; set; }

        public ApprenticeshipStatus ApprenticeshipStatus { get; set; }
        public string EmployerReference { get; set; }
        public bool IsContinuation { get; set; }
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateModel DateOfBirth { get; set; }

        [Display(Name = "Day")]
        [SuppressArgumentException(nameof(DateOfBirth), "The date of birth is not valid")]
        public int? BirthDay { get => DateOfBirth.Day; set => DateOfBirth.Day = value; }

        [Display(Name = "Month")]
        [SuppressArgumentException(nameof(DateOfBirth), "The date of birth is not valid")]
        public int? BirthMonth { get => DateOfBirth.Month; set => DateOfBirth.Month = value; }

        [Display(Name = "Year")]
        [SuppressArgumentException(nameof(DateOfBirth), "The date of birth is not valid")]
        public int? BirthYear { get => DateOfBirth.Year; set => DateOfBirth.Year = value; }

        public string CourseCode { get; set; }

        public IEnumerable<TrainingProgramme> Courses { get; set; }

        [Display(Name = "Planned training start date")]
        public MonthYearModel StartDate { get; set; }

        [Display(Name = "Month")]
        [SuppressArgumentException(nameof(StartDate), "The start date is not valid")]
        public int? StartMonth { get => StartDate.Month; set => StartDate.Month = value; }

        [Display(Name = "Year")]
        [SuppressArgumentException(nameof(StartDate), "The start date is not valid")]
        public int? StartYear { get => StartDate.Year; set => StartDate.Year = value; }

        [Display(Name = "Projected finish date")]
        public MonthYearModel EndDate { get; set; }

        [Display(Name = "Month")]
        [SuppressArgumentException(nameof(EndDate), "The end date is not valid")]
        public int? EndMonth { get => EndDate.Month; set => EndDate.Month = value; }

        [Display(Name = "Year")]
        [SuppressArgumentException(nameof(EndDate), "The end date is not valid")]
        public int? EndYear { get => EndDate.Year; set => EndDate.Year = value; }

        [Display(Name = "Reference (optional)")]
        public string Reference { get; set; }
        public bool IsLockedForUpdate { get; set; }

        public bool IsUpdateLockedForStartDateAndCourse { get; set; }
        public bool IsEndDateLockedForUpdate { get; internal set; }
    }
}

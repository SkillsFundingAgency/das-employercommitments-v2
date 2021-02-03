using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Attributes;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared
{
    public class DraftApprenticeshipViewModel
    {
        public DraftApprenticeshipViewModel(DateTime? dateOfBirth, DateTime? startDate, DateTime? endDate) : base()
        {
            DateOfBirth = dateOfBirth == null ? new DateModel() : new DateModel(dateOfBirth.Value);
            StartDate = startDate == null ? new MonthYearModel("") : new MonthYearModel($"{startDate.Value.Month}{startDate.Value.Year}");
            EndDate = endDate == null ? new MonthYearModel("") : new MonthYearModel($"{endDate.Value.Month}{endDate.Value.Year}");
        }

        public DraftApprenticeshipViewModel()
        {
            DateOfBirth = new DateModel();
            StartDate = new MonthYearModel("");
            EndDate = new MonthYearModel("");
        }

        public long ProviderId { get; set; }
        public string ProviderName { get; set; }

        public string CohortReference { get; set; }
        public long? CohortId { get; set; }

        public Guid? ReservationId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateModel DateOfBirth { get; }

        [Display(Name = "Day")]
        [SuppressArgumentException(nameof(DateOfBirth), "The date of birth is not valid")]
        public int? BirthDay { get => DateOfBirth.Day; set => DateOfBirth.Day = value; }

        [Display(Name = "Month")]
        [SuppressArgumentException(nameof(DateOfBirth), "The date of birth is not valid")]
        public int? BirthMonth { get => DateOfBirth.Month; set => DateOfBirth.Month = value; }

        [Display(Name = "Year")]
        [SuppressArgumentException(nameof(DateOfBirth), "The date of birth is not valid")]
        public int? BirthYear { get => DateOfBirth.Year; set => DateOfBirth.Year = value; }

        [Display(Name = "Unique Learner Number (ULN)")]
        public string Uln { get; set; }

        public string CourseCode { get; set; }

        [Display(Name = "Planned training start date")]
        public MonthYearModel StartDate { get; set; }

        [Display(Name = "Month")]
        [SuppressArgumentException(nameof(StartDate), "The start date is not valid")]
        public int? StartMonth { get => StartDate.Month; set => StartDate.Month = value; }

        [Display(Name = "Year")]
        [SuppressArgumentException(nameof(StartDate), "The start date is not valid")]
        public int? StartYear { get => StartDate.Year; set => StartDate.Year = value; }

        [Display(Name = "Projected finish date")]
        public MonthYearModel EndDate { get; }

        [Display(Name = "Month")]
        [SuppressArgumentException(nameof(EndDate), "The end date is not valid")]
        public int? EndMonth { get => EndDate.Month; set => EndDate.Month = value; }

        [Display(Name = "Year")]
        [SuppressArgumentException(nameof(EndDate), "The end date is not valid")]
        public int? EndYear { get => EndDate.Year; set => EndDate.Year = value; }

        [Display(Name = "Total agreed apprenticeship price (excluding VAT)")]
        [SuppressArgumentException(nameof(Cost), "The apprenticeship price is not valid")]
        public int? Cost { get; set; }

        [Display(Name = "Reference (optional)")]
        public string Reference { get; set; }

        public IReadOnlyList<ITrainingProgramme> Courses { get; set; }

        public bool IsContinuation { get; set; }
    }
}

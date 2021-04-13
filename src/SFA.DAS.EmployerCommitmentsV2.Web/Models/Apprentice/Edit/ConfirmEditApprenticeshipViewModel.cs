using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ConfirmEditApprenticeshipViewModel : BaseConfirmEdit
    {
        public bool? ConfirmChanges { get; set; }

        public BaseConfirmEdit OriginalApprenticeship { get; set; }
    }

    //public class ConfirmEditOriginalApprenticeship
    //{
    //    public string Name
    //    {
    //        get
    //        {
    //            return FirstName + " " + LastName;
    //        }
    //    }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string ApprenticeshipHashedId { get; set; }
    //    public string ULN { get; set; }
    //    public string TrainingName { get; set; }
    //    public decimal? Cost { get; set; }
    //    public string EmployerReference { get; set; }
    //    public DateTime DateOfBirth { get; set; }
    //    public string CourseCode { get; set; }
    //    public DateTime StartDate { get; set; }
    //    public DateTime EndDate { get; set; }
    //}
}

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

public class DataLockCourseChange
{
    public DateTime CurrentStartDate { get; set; }

    public DateTime? CurrentEndDate { get; set; }

    public DateTime IlrStartDate { get; set; }

    public DateTime? IlrEndDate { get; set; }

    public string CurrentTrainingProgram { get; set; }

    public string IlrTrainingProgram { get; set; }
}
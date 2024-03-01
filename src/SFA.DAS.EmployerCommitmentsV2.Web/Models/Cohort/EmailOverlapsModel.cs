namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class EmailOverlapsModel
{
    public int NumberOfEmailOverlaps { get; }

    public EmailOverlapsModel(int numberOfEmailOverlaps)
    {
        NumberOfEmailOverlaps = numberOfEmailOverlaps;
    }

    public string DisplayEmailOverlapsMessage => "You can’t use the same email address";
}
namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class EmailOverlapsModel
    {
        public int NumberOfEmailOverlaps { get; }

        public EmailOverlapsModel(int numberOfEmailOverlaps)
        {
            NumberOfEmailOverlaps = numberOfEmailOverlaps;
        }

        public string DisplayEmailOverlapsMessage => NumberOfEmailOverlaps == 1 ? "1 apprenticeship with an email issue" : $"{NumberOfEmailOverlaps} apprenticeships with email issues";
    }
}
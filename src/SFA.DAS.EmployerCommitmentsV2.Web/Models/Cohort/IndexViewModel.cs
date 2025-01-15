namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class IndexViewModel
{
    public string AccountHashedId { get; set; }
    public Guid? ReservationId { get; set; }
    public string AccountLegalEntityHashedId { get; set; }
    public string StartMonthYear { get; set; }
    public string CourseCode { get; set; }
    public Origin Origin { get; set; }
    public bool IsLevyFunded { get; set; }
    public Guid? ApprenticeshipSessionKey { get; set; }

    public virtual Dictionary<string, string> ToDictionary()
    {
        var dictionary = new Dictionary<string, string>
        {
            {nameof(AccountHashedId), AccountHashedId }         
        };

        if (ApprenticeshipSessionKey.HasValue)
            dictionary.Add(nameof(ApprenticeshipSessionKey), ApprenticeshipSessionKey.ToString());

        return dictionary;
    }  
}
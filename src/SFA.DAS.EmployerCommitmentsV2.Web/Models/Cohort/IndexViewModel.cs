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
    public Guid? AddApprenticeshipCacheKey { get; set; }

    public virtual Dictionary<string, string> ToDictionary(bool includeCacheKey = false)
    {
        var dictionary = new Dictionary<string, string>
        {
            {nameof(AccountHashedId), AccountHashedId }         
        };
          
        if (includeCacheKey)
        {
            if (AddApprenticeshipCacheKey.HasValue)
                dictionary.Add(nameof(AddApprenticeshipCacheKey), AddApprenticeshipCacheKey.ToString());
        }
        else
        {
            dictionary.Add(nameof(AccountLegalEntityHashedId), AccountLegalEntityHashedId);
            dictionary.Add(nameof(Origin), Origin.ToString());

            if (ReservationId.HasValue)
                dictionary.Add(nameof(ReservationId), ReservationId.ToString());
            if (!string.IsNullOrWhiteSpace(StartMonthYear))
                dictionary.Add(nameof(StartMonthYear), StartMonthYear);
            if (!string.IsNullOrWhiteSpace(CourseCode))
                dictionary.Add(nameof(CourseCode), CourseCode);
        }
        
        return dictionary;
    }  
}
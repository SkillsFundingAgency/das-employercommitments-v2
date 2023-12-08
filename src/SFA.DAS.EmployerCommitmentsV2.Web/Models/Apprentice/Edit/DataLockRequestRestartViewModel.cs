using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

public class DataLockRequestRestartViewModel : IAuthorizationContextModel
{
    [FromRoute]
    public string AccountHashedId { get; set; }
    [JsonIgnore]
    public long AccountId { get; set; }
    [FromRoute]
    public string ApprenticeshipHashedId { get; set; }
    [JsonIgnore]
    public long ApprenticeshipId { get; set; }
    public string ProviderName { get; set; }
    public BaseEdit OriginalApprenticeship { get; set; }
    public string NewCourseCode { get; internal set; }
    public string NewCourseName { get; internal set; }
    public DateTime? NewPeriodStartDate { get; internal set; }
    public DateTime? NewPeriodEndDate { get; internal set; }
}
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

public class DataLockRequestChangesViewModel : IAuthorizationContextModel
{
    public bool? AcceptChanges { get; set; }
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
    public IList<DataLockCourseChange> CourseChanges { get; internal set; }
    public IList<DataLockPriceChange> PriceChanges { get; internal set; }
}
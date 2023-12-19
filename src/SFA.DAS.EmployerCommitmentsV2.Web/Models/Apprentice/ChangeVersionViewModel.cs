using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class ChangeVersionViewModel : IAuthorizationContextModel
{
    public string AccountHashedId { get; set; }
    public string ApprenticeshipHashedId { get; set; }
    public long ApprenticeshipId { get; set; }

    public string StandardTitle { get; set; }
    public string StandardUrl { get; set; }
    public string CurrentVersion { get; set; }
    public string SelectedVersion { get; set; }
    public IEnumerable<string> NewerVersions { get; set; }
}
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class Agreement
{
    public long Id { get; set; }
    public DateTime? SignedDate { get; set; }
    public string SignedByName { get; set; }
    public EmployerAgreementStatus Status { get; set; }
    public int TemplateVersionNumber { get; set; }
}
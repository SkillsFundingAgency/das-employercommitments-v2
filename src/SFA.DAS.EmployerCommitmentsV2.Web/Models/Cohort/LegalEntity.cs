namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class LegalEntity
{
    public string Name { get; set; }
    public string RegisteredAddress { get; set; }
    public short Source { get; set; }
    public List<Agreement> Agreements { get; set; }
    public long Id { get; set; }
    public string Code { get; set; }
    public string AccountLegalEntityPublicHashedId { get; set; }
}
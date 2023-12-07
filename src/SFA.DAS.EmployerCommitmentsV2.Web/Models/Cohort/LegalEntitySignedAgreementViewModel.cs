using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class LegalEntitySignedAgreementViewModel : BaseLegalEntitySignedAgreementViewModel
{
    public long AccountLegalEntityId { get; set; }
}

public class BaseLegalEntitySignedAgreementViewModel : IAuthorizationContextModel
{
    public string AccountHashedId { get; set; }
    public string TransferConnectionCode { get; set; }
    public string CohortRef { get; set; }
    public bool HasSignedMinimumRequiredAgreementVersion { get; set; }
    public string LegalEntityName { get; set; }
    public string AccountLegalEntityHashedId { get; set; }
    public string EncodedPledgeApplicationId { get; set; }

    public BaseLegalEntitySignedAgreementViewModel CloneBaseValues() =>
        this.ExplicitClone();
}
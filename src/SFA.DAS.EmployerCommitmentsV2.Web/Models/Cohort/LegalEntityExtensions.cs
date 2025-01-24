namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public static class LegalEntityExtensions
{

    public static LegalEntity MapToLegalEntityVm(this EmployerCommitmentsV2.Services.Approvals.Responses.LegalEntity input)
    {
        return new LegalEntity
        {
            Name = input.Name,
            RegisteredAddress = input.Address,
            Id = input.LegalEntityId,
            AccountLegalEntityPublicHashedId = input.AccountLegalEntityPublicHashedId,
            Agreements = input.Agreements.ConvertAll(MapToAgreementVm)
        };
    }

    public static Agreement MapToAgreementVm(this EmployerCommitmentsV2.Services.Approvals.Responses.Agreement input)
    {
        return new Agreement
        {
            Id = input.Id,
            Status = input.Status,
            TemplateVersionNumber = input.TemplateVersionNumber
        };
    }
}
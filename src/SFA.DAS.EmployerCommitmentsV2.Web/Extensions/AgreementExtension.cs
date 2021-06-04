using System.Linq;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class AgreementExtension
    {   
        public static bool HasSignedAgreement(this LegalEntity legalEntity, bool isTransfer)
        {
            if (isTransfer)
            {
                return legalEntity.Agreements.Any(a =>
                    a.Status == EmployerAgreementStatus.Signed && a.TemplateVersionNumber >= 2);
            }

            return legalEntity.Agreements.Any(a =>
                    a.Status == EmployerAgreementStatus.Signed && a.TemplateVersionNumber >= 1);
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Features;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Services
{

    /// <summary>
    ///     A service that can be used to redirect to version 1 or 2 of specified pages.
    /// </summary>
    public interface IUrlSelectorService
    {
        /// <summary>
        ///     Redirect to version 2 of the CohortDetails page if the <see cref="EmployerFeature.EnhancedApproval"/> is enabled
        ///     otherwise redirect to version 1.
        /// </summary>
        /// <returns>
        ///     An action result that redirects to the relevant page. 
        /// </returns>
        ActionResult RedirectToCohortDetails(IUrlHelper urlHelper, string accountHashedId, string cohortReference);

        /// <summary>
        ///     Redirects to version 1 of the CohortDetails page for the supplied account and cohort if the supplied cohort is
        ///     with the provider.
        /// </summary>
        ActionResult RedirectToV1IfCohortWithOtherParty(string accountHashId, string cohortReference, GetCohortResponse cohort);
    }
}
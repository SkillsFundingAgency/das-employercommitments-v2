using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ChangeProviderRequestedConfirmationRequest
    {
        [FromRoute]
        public string AccountHashedId { get; set; }

        [FromRoute]
        public string ApprenticeshipHashedId { get; set; }

        [FromRoute]
        public long ProviderId{ get; set; }


    }
}

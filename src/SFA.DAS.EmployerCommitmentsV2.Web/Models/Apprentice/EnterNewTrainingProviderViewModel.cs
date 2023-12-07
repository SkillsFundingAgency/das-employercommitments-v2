using SFA.DAS.CommitmentsV2.Api.Types.Responses;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class EnterNewTrainingProviderViewModel : ChangeOfProviderBaseViewModel
{
    public long CurrentProviderId { get; set; }
    public List<Provider> Providers { get; set; }
}
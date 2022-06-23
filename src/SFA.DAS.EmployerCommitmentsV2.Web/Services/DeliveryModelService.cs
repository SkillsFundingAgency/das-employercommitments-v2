using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Services
{
    public class DeliveryModelService : IDeliveryModelService
    {
        private readonly IApprovalsApiClient _approvalsApiClient;

        public DeliveryModelService(IApprovalsApiClient approvalsApiClient)
        {
            _approvalsApiClient = approvalsApiClient;
        }

        public async Task<bool> HasMultipleDeliveryModels(long providerId, string courseCode)
        {
            var response = await _approvalsApiClient.GetProviderCourseDeliveryModels(providerId, courseCode);
            return (response?.DeliveryModels.Count() > 1);
        }
    }
}

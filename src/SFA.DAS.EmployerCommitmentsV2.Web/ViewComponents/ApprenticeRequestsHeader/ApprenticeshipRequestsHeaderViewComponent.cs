using Microsoft.AspNetCore.Mvc;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ViewComponents.ApprenticeRequestsHeader
{
    public class ApprenticeshipRequestsHeaderViewComponent : ViewComponent
    {
        private readonly IModelMapper _modelMapper;

        public ApprenticeshipRequestsHeaderViewComponent(IModelMapper modelMapper)
        {
            _modelMapper = modelMapper;
        }

        public async Task<IViewComponentResult> InvokeASync(CohortsByAccountRequest request)
        {
            var model = await _modelMapper.Map<CohortsViewModel>(request);
            return View(model);
        }
    }
}

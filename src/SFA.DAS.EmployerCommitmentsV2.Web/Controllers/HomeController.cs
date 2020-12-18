using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Home;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [DasAuthorize(EmployerUserRole.OwnerOrTransactor)]
    [Route("{accountHashedId}")]
    public class HomeController : Controller
    {
        private readonly IModelMapper _modelMapper;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IModelMapper modelMapper, ILogger<HomeController> logger)
        {
            _modelMapper = modelMapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index(IndexRequest request)
        {
            try
            {
                var model = await _modelMapper.Map<IndexViewModel>(request);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed '{nameof(HomeController)}': {nameof(ex.Message)}='{ex.Message}', {nameof(ex.StackTrace)}='{ex.StackTrace}'");
                return RedirectToAction("Error", "Error");
            }
        }
    }
}
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Home;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers;

[Authorize(Policy = nameof(PolicyNames.HasEmployerViewerTransactorOwnerAccount))]
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

    [HttpGet]
    public async Task<IActionResult> Index(IndexRequest request)
    {
        _logger.LogInformation("Home.Index() action called for request: {Request}", JsonSerializer.Serialize(request));
        
        var model = await _modelMapper.Map<IndexViewModel>(request);
        return View(model);
    }
}
﻿using Microsoft.AspNetCore.Authorization;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Home;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers;

[Authorize(Policy = nameof(PolicyNames.HasEmployerTransactorOwnerAccount))]
[Route("{accountHashedId}")]
public class HomeController : Controller
{
    private readonly IModelMapper _modelMapper;

    public HomeController(IModelMapper modelMapper) => _modelMapper = modelMapper;

    [HttpGet]
    public async Task<IActionResult> Index(IndexRequest request)
    {
        var model = await _modelMapper.Map<IndexViewModel>(request);
        return View(model);
    }
}
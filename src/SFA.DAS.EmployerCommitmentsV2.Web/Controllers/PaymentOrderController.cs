﻿using Microsoft.AspNetCore.Authorization;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.PaymentOrder;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers;

[Authorize(Policy = nameof(PolicyNames.HasEmployerTransactorOwnerAccount))]
[Route("accounts/{accountHashedId}/apprentices/manage")]
public class PaymentOrderController : Controller
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly ILogger<PaymentOrderController> _logger;
    private readonly IModelMapper _modelMapper;

    public PaymentOrderController(
        ICommitmentsApiClient commitmentsApiClient,
        ILogger<PaymentOrderController> logger,
        IModelMapper modelMapper)
    {
        _commitmentsApiClient = commitmentsApiClient;
        _logger = logger;
        _modelMapper = modelMapper;
    }

    [HttpGet]
    [Route("paymentorder", Name = "ProviderPaymentOrder")]
    public async Task<ActionResult> ProviderPaymentOrder(PaymentOrderRequest request)
    {
        var viewModel = await _modelMapper.Map<PaymentOrderViewModel>(request);
        return View(viewModel);
    }

    [Route("paymentorder")]
    [HttpPost]
    public async Task<IActionResult> ProviderPaymentOrder(PaymentOrderViewModel viewModel)
    {
        try
        {
            var request = await _modelMapper.Map<UpdateProviderPaymentsPriorityRequest>(viewModel);
            await _commitmentsApiClient.UpdateProviderPaymentsPriority(viewModel.AccountId, request);
                
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).ControllerName(), new { viewModel.AccountHashedId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                $"Failed to set payment order '{nameof(PaymentOrderController)}-{nameof(ProviderPaymentOrder)}'");
        }

        return RedirectToAction("Error", "Error");
    }
}
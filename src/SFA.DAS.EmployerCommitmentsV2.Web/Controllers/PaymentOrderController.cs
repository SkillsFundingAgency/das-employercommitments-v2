using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.PaymentOrder;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers;

[Authorize(Policy = nameof(PolicyNames.HasEmployerTransactorOwnerAccount))]
[Route("accounts/{accountHashedId}/apprentices/manage")]
public class PaymentOrderController(
    ICommitmentsApiClient commitmentsApiClient,
    ILogger<PaymentOrderController> logger,
    IModelMapper modelMapper)
    : Controller
{
    [HttpGet]
    [Route("paymentorder", Name = "ProviderPaymentOrder")]
    public async Task<ActionResult> ProviderPaymentOrder(PaymentOrderRequest request)
    {
        var viewModel = await modelMapper.Map<PaymentOrderViewModel>(request);
        return View(viewModel);
    }

    [Route("paymentorder")]
    [HttpPost]
    public async Task<IActionResult> ProviderPaymentOrder(PaymentOrderViewModel viewModel)
    {
        logger.LogInformation("ProviderPaymentOrderController.ProviderPaymentOrder POST called. Model: {Model}", JsonConvert.SerializeObject(viewModel));
        
        try
        {
            var request = await modelMapper.Map<UpdateProviderPaymentsPriorityRequest>(viewModel);
            
            logger.LogInformation("ProviderPaymentOrderController.ProviderPaymentOrder request object after mapping: {Model}", JsonConvert.SerializeObject(request));
            
            await commitmentsApiClient.UpdateProviderPaymentsPriority(viewModel.AccountId, request);

            logger.LogInformation("ProviderPaymentOrderController.ProviderPaymentOrder completed save to commitmentsApiClient");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Failed to set payment order '{nameof(PaymentOrderController)}-{nameof(ProviderPaymentOrder)}'");
            
            logger.LogInformation("ProviderPaymentOrderController.ProviderPaymentOrder caught in exception {Ex}, redirecting to error controller.", ex.ToString());
            
            return RedirectToAction("Error", "Error");
        }
       
        logger.LogInformation("ProviderPaymentOrderController.ProviderPaymentOrder Redirecting to HomeController.Index");
        
        return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).ControllerName(), new { viewModel.AccountHashedId });
    }
}
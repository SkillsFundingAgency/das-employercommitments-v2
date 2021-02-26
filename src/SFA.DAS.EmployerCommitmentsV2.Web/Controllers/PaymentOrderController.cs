using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.PaymentOrder;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [DasAuthorize(EmployerUserRole.OwnerOrTransactor)]
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
        [Route("paymentorder", Name = "PaymentOrder")]
        public async Task<ActionResult> PaymentOrder(PaymentOrderRequest request)
        {
            var viewModel = await _modelMapper.Map<PaymentOrderViewModel>(request);
            return View(viewModel);
        }

        [Route("paymentorder")]
        [HttpPost]
        public async Task<IActionResult> PaymentOrder([FromServices] IAuthenticationService authenticationService, PaymentOrderViewModel viewModel)
        {
            try
            {
                await _commitmentsApiClient.UpdateProviderPaymentsPriority(viewModel.AccountId,
                        new UpdateProviderPaymentsPriorityRequest
                        {
                            ProviderPriorities = viewModel.ProviderPaymentOrder
                                .Select((p, index) => new UpdateProviderPaymentsPriorityRequest.ProviderPaymentPriorityUpdateItem
                                {
                                    ProviderId = long.Parse(p),
                                    PriorityOrder = index + 1
                                })
                                .ToList(),
                            UserInfo = authenticationService.UserInfo
                        });

                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).ControllerName(), new { viewModel.AccountHashedId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"Failed to set payment order '{nameof(PaymentOrderController)}-{nameof(PaymentOrder)}'");
            }

            return RedirectToAction("Error", "Error");
        }
    }
}

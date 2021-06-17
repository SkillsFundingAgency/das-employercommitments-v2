using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [DasAuthorize(EmployerUserRole.OwnerOrTransactor)]
    [Route("{accountHashedId}")]
    public class TransferRequestController : Controller
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly ILogger<TransferRequestController> _logger;
        private readonly IModelMapper _modelMapper;

        public TransferRequestController(
            ICommitmentsApiClient commitmentsApiClient,
            ILogger<TransferRequestController> logger,
            IModelMapper modelMapper)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _logger = logger;
            _modelMapper = modelMapper;
        }

        [HttpGet]
        [Route("sender/transfers/{transferRequestHashedId}")]
        public async Task<IActionResult> TransferDetailsForSender(TransferRequestRequest request)
        {
            _logger.LogInformation($"Getting TransferRequest Details, Transfer Account: {request.AccountId}, TransferRequestId: {request.TransferRequestId}");

            var viewModel = await _modelMapper.Map<TransferRequestForSenderViewModel>(request);
            return View(viewModel);
        }

        [HttpPost]
        [Route("sender/transfers/{transferRequestHashedId}")]
        public async Task<IActionResult> TransferDetailsForSender(TransferRequestForSenderViewModel viewModel)
        {
            _logger.LogInformation($"Updating TransferRequest, Account: {viewModel.TransferSenderHashedAccountId}, CohortReference: {viewModel.HashedCohortReference}");

            try
            {
                var request = await _modelMapper.Map<UpdateTransferApprovalForSenderRequest>(viewModel);
                await _commitmentsApiClient.UpdateTransferRequestForSender(request.TransferSenderId, request.TransferRequestId, request.CohortId, request);

                return RedirectToAction(nameof(TransferRequestController.TransferConfirmation), nameof(TransferRequestController).ControllerName());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update transfer request");
            }

            return RedirectToAction("Error", "Error");
        }

        [HttpGet]
        [Route("sender/transfers/{transferRequestHashedId}/confirmation")]
        public async Task<IActionResult> TransferConfirmation(TransferConfirmationRequest request)
        {
            var viewModel = await _modelMapper.Map<TransferConfirmationViewModel>(request);
            return View(viewModel);
        }

        [HttpPost]
        [Route("sender/transfers/{transferRequestHashedId}/confirmation")]
        public async Task<ActionResult> TransferConfirmation(TransferConfirmationViewModel viewModel)
        {
            var request = await _modelMapper.Map<TransferConfirmationWhatNextRequest>(viewModel);
            return Redirect(request.WhatNextUrl);
        }

        [HttpGet]
        [Route("receiver/transfers/{transferRequestHashedId}")]
        public async Task<IActionResult> TransferDetailsForReceiver(TransferRequestRequest request)
        {
            _logger.LogInformation($"Getting TransferRequest Details, Transfer Account: {request.AccountId}, TransferRequestId: {request.TransferRequestId}");

            var viewModel = await _modelMapper.Map<TransferRequestForReceiverViewModel>(request);
            return View(viewModel);
        }
    }
}

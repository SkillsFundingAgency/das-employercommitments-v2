using System;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [DasAuthorize(EmployerUserRole.Owner)]
    [Route("{accountHashedId}/unapproved/add")]
    public class CreateCohortController : Controller
    {
        private readonly IMapper<IndexRequest, IndexViewModel> _indexViewModelMapper;
        private readonly IMapper<SelectProviderRequest, SelectProviderViewModel> _selectProviderViewModelMapper;
        private readonly IMapper<SelectProviderViewModel, ConfirmProviderRequest> _confirmProviderRequestMapper;
        private readonly IMapper<ConfirmProviderRequest, ConfirmProviderViewModel> _confirmProviderViewModelMapper;
        private readonly IValidator<SelectProviderViewModel> _selectProviderViewModelValidator;
        private readonly ILinkGenerator _linkGenerator;
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly ILogger<CreateCohortController> _logger;

        public CreateCohortController(
            IMapper<IndexRequest, IndexViewModel> indexViewModelMapper,
            IMapper<SelectProviderRequest, SelectProviderViewModel> selectProviderViewModelMapper,
            IMapper<SelectProviderViewModel, ConfirmProviderRequest> confirmProviderRequestMapper,
            IMapper<ConfirmProviderRequest, ConfirmProviderViewModel> confirmProviderViewModelMapper,
            IValidator<SelectProviderViewModel> selectProviderViewModelValidator,
            ILinkGenerator linkGenerator,
            ICommitmentsApiClient commitmentsApiClient,
            ILogger<CreateCohortController> logger)
        {
            _indexViewModelMapper = indexViewModelMapper;
            _selectProviderViewModelMapper = selectProviderViewModelMapper;
            _confirmProviderRequestMapper = confirmProviderRequestMapper;
            _confirmProviderViewModelMapper = confirmProviderViewModelMapper;
            _selectProviderViewModelValidator = selectProviderViewModelValidator;
            _linkGenerator = linkGenerator;
            _commitmentsApiClient = commitmentsApiClient;
            _logger = logger;
        }

        public IActionResult Index(IndexRequest request)
        {
            var viewModel = _indexViewModelMapper.Map(request);

            viewModel.OrganisationsLink = _linkGenerator.YourOrganisationsAndAgreements(request.AccountHashedId);
            viewModel.PayeSchemesLink = _linkGenerator.PayeSchemes(request.AccountHashedId);

            return View(viewModel);
        }

        [Route("select-provider")]
        public IActionResult SelectProvider(SelectProviderRequest request)
        {
            var viewModel = _selectProviderViewModelMapper.Map(request);

            return View(viewModel);
        }

        [Route("select-provider")]
        [HttpPost]
        public async Task<IActionResult> SelectProvider(SelectProviderViewModel request)
        {
            try
            {
                var validationResult = _selectProviderViewModelValidator.Validate(request);

                if (!validationResult.IsValid)
                {
                    return View(request);
                }

                await _commitmentsApiClient.GetProvider(long.Parse(request.ProviderId));

                var confirmProviderRequest = _confirmProviderRequestMapper.Map(request);

                return RedirectToAction("ConfirmProvider", confirmProviderRequest);
            }
            catch (RestHttpClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    ModelState.AddModelError(nameof(request.ProviderId), "Check UK Provider Reference Number");
                    return View(request);
                }
                _logger.LogError($"Failed '{nameof(CreateCohortController)}-{nameof(SelectProvider)}': {nameof(ex.StatusCode)}='{ex.StatusCode}', {nameof(ex.ReasonPhrase)}='{ex.ReasonPhrase}'");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed '{nameof(CreateCohortController)}-{nameof(SelectProvider)}': {nameof(ex.Message)}='{ex.Message}', {nameof(ex.StackTrace)}='{ex.StackTrace}'");
            }

            return RedirectToAction("Error","Error");
        }

        [Route("confirm-provider")]
        [HttpGet]
        public async Task<IActionResult> ConfirmProvider(ConfirmProviderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Error");
            }
            var response = await _commitmentsApiClient.GetProvider(request.ProviderId);
            
            var model = _confirmProviderViewModelMapper.Map(request);
            model.ProviderId = response.ProviderId;
            model.ProviderName = response.Name;
            
            return View(model);
        }

        [Route("confirm-provider")]
        [HttpPost]
        public IActionResult ConfirmProvider(ConfirmProviderViewModel request)
        {
            return RedirectToRoute("");
        }
    }
}
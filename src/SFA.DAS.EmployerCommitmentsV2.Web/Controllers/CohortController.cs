using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [DasAuthorize(EmployerFeature.EmployerCommitmentsV2, EmployerUserRole.OwnerOrTransactor)]
    [Route("{accountHashedId}/unapproved/add")]
    public class CohortController : Controller
    {
        private readonly IMapper<IndexRequest, IndexViewModel> _indexViewModelMapper;
        private readonly IMapper<SelectProviderRequest, SelectProviderViewModel> _selectProviderViewModelMapper;
        private readonly IMapper<SelectProviderViewModel, ConfirmProviderRequest> _confirmProviderRequestMapper;
        private readonly IMapper<ConfirmProviderRequest, ConfirmProviderViewModel> _confirmProviderViewModelMapper;
		private readonly IMapper<AssignRequest, AssignViewModel> _assignViewModelMapper;
        private readonly IMapper<ConfirmProviderViewModel, SelectProviderViewModel> _selectProviderFromConfirmMapper;
        private readonly IMapper<ConfirmProviderViewModel, AssignRequest> _assignRequestMapper;
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly ILogger<CohortController> _logger;

        public CohortController(
            IMapper<IndexRequest, IndexViewModel> indexViewModelMapper,
            IMapper<SelectProviderRequest, SelectProviderViewModel> selectProviderViewModelMapper,
            IMapper<SelectProviderViewModel, ConfirmProviderRequest> confirmProviderRequestMapper,
			IMapper<ConfirmProviderRequest, ConfirmProviderViewModel> confirmProviderViewModelMapper,
            IMapper<ConfirmProviderViewModel, SelectProviderViewModel> selectProviderFromConfirmMapper,
            IMapper<ConfirmProviderViewModel, AssignRequest> assignRequestMapper,
            IMapper<AssignRequest, AssignViewModel> assignViewModelMapper,
            ICommitmentsApiClient commitmentsApiClient,
            ILogger<CohortController> logger)
        {
            _indexViewModelMapper = indexViewModelMapper;
 			_selectProviderViewModelMapper = selectProviderViewModelMapper;
			_assignViewModelMapper = assignViewModelMapper;
            _confirmProviderRequestMapper = confirmProviderRequestMapper;
            _confirmProviderViewModelMapper = confirmProviderViewModelMapper;
            _selectProviderFromConfirmMapper = selectProviderFromConfirmMapper;
            _assignRequestMapper = assignRequestMapper;
            _commitmentsApiClient = commitmentsApiClient;
            _logger = logger;
        }

        public IActionResult Index(IndexRequest request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Error");
            }

            var viewModel = _indexViewModelMapper.Map(request);

            return View(viewModel);
        }

        [Route("select-provider")]
        public IActionResult SelectProvider(SelectProviderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Error", new { StatusCode = 400 });
            }

            var viewModel = _selectProviderViewModelMapper.Map(request);

            return View(viewModel);
        }

        [Route("select-provider")]
        [HttpPost]
        public async Task<IActionResult> SelectProvider(SelectProviderViewModel request)
        {
            try
            {
                if (!ModelState.IsValid)
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

                _logger.LogError(
                    $"Failed '{nameof(CohortController)}-{nameof(SelectProvider)}': {nameof(ex.StatusCode)}='{ex.StatusCode}', {nameof(ex.ReasonPhrase)}='{ex.ReasonPhrase}'");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Failed '{nameof(CohortController)}-{nameof(SelectProvider)}': {nameof(ex.Message)}='{ex.Message}', {nameof(ex.StackTrace)}='{ex.StackTrace}'");
            }

            return RedirectToAction("Error", "Error");
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
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            if (request.UseThisProvider.Value)
            {
                var model = _assignRequestMapper.Map(request);
                return RedirectToAction("assign", model);
            }

            var returnModel = _selectProviderFromConfirmMapper.Map(request);

            return RedirectToAction("SelectProvider", returnModel);
        }

        [Route("assign")]
        public IActionResult Assign(AssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Error");
            }

            var viewModel = _assignViewModelMapper.Map(request);

            return View(viewModel);
        }

        [Route("assign")]
        [HttpPost]
        public IActionResult Assign(AssignViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var routeValues = new
            {
                model.AccountHashedId,
                model.AccountLegalEntityHashedId,
                model.ReservationId,
                model.StartMonthYear,
                model.CourseCode,
                model.ProviderId
            };

            switch (model.WhoIsAddingApprentices)
            {
                case WhoIsAddingApprentices.Employer:
                    return RedirectToAction("AddDraftApprenticeship", "CreateCohortWithDraftApprenticeship", routeValues);
                case WhoIsAddingApprentices.Provider:
                    return RedirectToAction("Message", "CreateCohortWithOtherParty", routeValues);
                default:
                    return RedirectToAction("Error", "Error");
            }
        }
    }
}
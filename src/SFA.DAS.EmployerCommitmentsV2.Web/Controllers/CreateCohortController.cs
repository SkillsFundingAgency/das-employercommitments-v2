using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;
using SFA.DAS.EmployerUrlHelper;
using StructureMap.Query;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [DasAuthorize(EmployerUserRole.Owner)]
    [Route("{accountHashedId}/unapproved/add")]
    public class CreateCohortController : Controller
    {
        private readonly IMapper<IndexRequest, IndexViewModel> _indexViewModelMapper;
        private readonly IMapper<SelectProviderRequest, SelectProviderViewModel> _selectProviderViewModelMapper;
        private readonly IMapper<SelectProviderViewModel, ConfirmProviderRequest> _confirmProviderRequestMapper;
        private readonly IValidator<SelectProviderViewModel> _selectProviderViewModelValidator;
        private readonly ILinkGenerator _linkGenerator;
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public CreateCohortController(
            IMapper<IndexRequest, IndexViewModel> indexViewModelMapper,
            IMapper<SelectProviderRequest, SelectProviderViewModel> selectProviderViewModelMapper,
            IMapper<SelectProviderViewModel, ConfirmProviderRequest> confirmProviderRequestMapper,
            IValidator<SelectProviderViewModel> selectProviderViewModelValidator,
            ILinkGenerator linkGenerator, 
            ICommitmentsApiClient commitmentsApiClient)
        {
            _indexViewModelMapper = indexViewModelMapper;
            _selectProviderViewModelMapper = selectProviderViewModelMapper;
            _confirmProviderRequestMapper = confirmProviderRequestMapper;
            _selectProviderViewModelValidator = selectProviderViewModelValidator;
            _linkGenerator = linkGenerator;
            _commitmentsApiClient = commitmentsApiClient;
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
            var validationResult = _selectProviderViewModelValidator.Validate(request);

            if (!validationResult.IsValid)
            {
                return View(request);
            }

            GetProviderResponse providerResponse;
            try
            {
                providerResponse = await _commitmentsApiClient.GetProvider(long.Parse(request.ProviderId));
            }
            catch (Exception)
            {
                ModelState.AddModelError(nameof(providerResponse.ProviderId), "Check UK Provider Reference Number");
                return View(request);
            }

            var confirmProviderRequest = _confirmProviderRequestMapper.Map(request);

            return RedirectToAction("ConfirmProvider", confirmProviderRequest);
        }

        [Route("confirm-provider")]
        [HttpGet]
        public IActionResult ConfirmProvider(ConfirmProviderRequest request)
        {
            return View(request);
        }
    }
}
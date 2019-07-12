using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Application.Queries.Providers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [DasAuthorize(EmployerUserRole.Owner)]
    [Route("{accountHashedId}/unapproved/add")]
    public class CreateCohortController : Controller
    {
        private readonly IMapper<IndexRequest, IndexViewModel> _indexViewModelMapper;
        private readonly ILinkGenerator _linkGenerator;
        private readonly IMediator _mediator;

        public CreateCohortController(
            IMapper<IndexRequest, IndexViewModel> indexViewModelMapper,
            ILinkGenerator linkGenerator,
            IMediator mediator)
        {
            _indexViewModelMapper = indexViewModelMapper;
            _linkGenerator = linkGenerator;
            _mediator = mediator;
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
            var viewModel = new SelectProviderViewModel();//todo: from mapper

            return View(viewModel);
        }

        [Route("select-provider")]
        [HttpPost]
        public IActionResult SelectProvider(SelectProviderViewModel request)
        {
            //todo:hit api

            var confirmProviderRequest = new ConfirmProviderRequest();//todo: from mapper

            return RedirectToAction("ConfirmProvider", confirmProviderRequest);
        }

        [Route("confirm-provider")]
        [HttpGet]
        public async Task<IActionResult> ConfirmProvider(ConfirmProviderRequest request)
        {
            var response = await _mediator.Send(new GetProviderRequest{UkPrn = request.ProviderId});

            var model = new ConfirmProviderViewModel
            {
                ProviderId = response.ProviderId,
                ProviderName = response.ProviderName
            };

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
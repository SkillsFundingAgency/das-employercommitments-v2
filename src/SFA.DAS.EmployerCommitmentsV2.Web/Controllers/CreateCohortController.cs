using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Commitments.Shared.Interfaces;
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

        public CreateCohortController(
            IMapper<IndexRequest, IndexViewModel> indexViewModelMapper,
            ILinkGenerator linkGenerator)
        {
            _indexViewModelMapper = indexViewModelMapper;
            _linkGenerator = linkGenerator;
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
        public IActionResult ConfirmProvider(ConfirmProviderRequest request)
        {
            return View(request);
        }
    }
}
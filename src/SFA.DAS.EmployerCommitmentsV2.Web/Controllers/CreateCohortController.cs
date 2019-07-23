using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Commitments.Shared.Extensions;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [DasAuthorize(EmployerUserRole.OwnerOrTransactor)]
    [Route("{accountHashedId}/unapproved/add")]
    public class CreateCohortController : Controller
    {
        private readonly IMapper<IndexRequest, IndexViewModel> _indexViewModelMapper;
        private readonly IMapper<AssignRequest, AssignViewModel> _assignViewModelMapper;
        private readonly ILinkGenerator _linkGenerator;
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public CreateCohortController(
            IMapper<IndexRequest, IndexViewModel> indexViewModelMapper,
            IMapper<AssignRequest, AssignViewModel> assignViewModelMapper,
            ILinkGenerator linkGenerator, ICommitmentsApiClient commitmentsApiClient )
        {
            _indexViewModelMapper = indexViewModelMapper;
            _assignViewModelMapper = assignViewModelMapper;
            _linkGenerator = linkGenerator;
            _commitmentsApiClient = commitmentsApiClient;
        }

        public IActionResult Index(IndexRequest request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Error");
            }

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
                model.EmployerAccountLegalEntityPublicHashedId,
                model.ReservationId,
                model.StartMonthYear,
                model.CourseCode,
                model.UkPrn
            };

            switch (model.WhoIsAddingApprentices)
            {
                case WhoIsAddingApprentices.Employer:
                    return RedirectToAction("Apprentice", routeValues);
                case WhoIsAddingApprentices.Provider:
                    return RedirectToAction("Message", routeValues);
                default:
                    return RedirectToAction("Error", "Error");
            }
        }

        [Route("apprentice")]
        public IActionResult Apprentice(ApprenticeRequest request)
        {

            return View();
        }

        [Route("message")]
        public async Task<IActionResult> Message(MessageRequest request)
        {
            var messageModel = new MessageViewModel
            {
                AccountHashedId = request.AccountHashedId,
                AccountLegalEntityHashedId = request.EmployerAccountLegalEntityPublicHashedId,
                ProviderId = request.UkPrn,
                ReservationId = request.ReservationId,
            };
            messageModel.ProviderName = await GetProviderName(messageModel.ProviderId);

            return View(messageModel);
        }

        [HttpPost]
        [Route("message")]
        public async Task<IActionResult> Message(MessageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ProviderName = await GetProviderName(model.ProviderId);
                return View(model);
            }

            try
            {
                // Post Create Request to CommitmentsAPI 
                //var cohortReference = _commitmentsApiClient.CreateCohort(xxxx);
                return View(model);

            }
            catch (CommitmentsApiModelException ex)
            {
                ModelState.AddModelExceptionErrors(ex);
                model.ProviderName = await GetProviderName(model.ProviderId);
                return View(model);
            }

            
        }

        private async Task<string> GetProviderName(long providerId)
        {
            return (await _commitmentsApiClient.GetProvider(providerId)).Name;
        }

    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Commitments.Shared.Extensions;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [DasAuthorize(EmployerUserRole.OwnerOrTransactor)]
    [Route("{accountHashedId}/unapproved/add")]
    public class CreateCohortWithOtherPartyController : Controller
    {
        private readonly IMapper<MessageViewModel, CreateCohortWithOtherPartyRequest> _createCohortWithOtherPartyMapper;
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public CreateCohortWithOtherPartyController(
            IMapper<MessageViewModel, CreateCohortWithOtherPartyRequest> createCohortWithOtherPartyMapper,
            ICommitmentsApiClient commitmentsApiClient )
        {
            _createCohortWithOtherPartyMapper = createCohortWithOtherPartyMapper;
            _commitmentsApiClient = commitmentsApiClient;
        }

        [Route("message")]
        public async Task<IActionResult> Message(MessageRequest request)
        {
            var messageModel = new MessageViewModel
            {
                AccountHashedId = request.AccountHashedId,
                AccountLegalEntityHashedId = request.EmployerAccountLegalEntityPublicHashedId,
                ProviderId = request.UkPrn,
                StartMonthYear = request.StartMonthYear,
                CourseCode = request.CourseCode,
                ReservationId = request.ReservationId
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
                var request = _createCohortWithOtherPartyMapper.Map(model);
                await _commitmentsApiClient.CreateCohort(request);
                return RedirectToAction("Finished");
            }
            catch (CommitmentsApiModelException ex)
            {
                ModelState.AddModelExceptionErrors(ex);
                model.ProviderName = await GetProviderName(model.ProviderId);
                return View(model);
            }
        }

        [Route("finished")]
        public IActionResult Finished()
        {
            return View();
        }

        private async Task<string> GetProviderName(long providerId)
        {
            return (await _commitmentsApiClient.GetProvider(providerId)).Name;
        }
    }
}
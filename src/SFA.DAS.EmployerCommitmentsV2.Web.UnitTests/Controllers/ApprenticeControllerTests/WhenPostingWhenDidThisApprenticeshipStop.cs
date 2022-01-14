using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    [TestFixture]
    public class WhenPostingWhenDidThisApprenticeshipStop : ApprenticeControllerTestBase
    {
        [SetUp]
        public void Arrange()
        {
            _controller = new ApprenticeController(Mock.Of<IModelMapper>(),
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                Mock.Of<ICommitmentsApiClient>(),
                Mock.Of<ILogger<ApprenticeController>>());
        }

        [Test]
        public void AndStopDateIsEntered_ThenRedirectToHasApprenticeBeenMadeRedundantPage()
        {
            StopRequestViewModel viewModel = GetStopRequestViewModel();
            var result = _controller.StopApprenticeship(viewModel);

            var redirect = result.VerifyReturnsRedirectToActionResult();
            Assert.AreEqual(redirect.ActionName, "HasTheApprenticeBeenMadeRedundant");
            Assert.AreEqual(redirect.RouteValues["AccountHashedId"], viewModel.AccountHashedId);
            Assert.AreEqual(redirect.RouteValues["ApprenticeshipHashedId"], viewModel.ApprenticeshipHashedId);
            Assert.AreEqual(redirect.RouteValues["StopMonth"], viewModel.StopMonth);
            Assert.AreEqual(redirect.RouteValues["StopYear"], viewModel.StopYear);
            Assert.AreEqual(redirect.RouteValues["IsCoPJourney"], viewModel.IsCoPJourney);
        }

        private static StopRequestViewModel GetStopRequestViewModel()
        {
            return new StopRequestViewModel { StopMonth = 6, StopYear = 2020, ApprenticeshipId = 1, AccountHashedId = "AAXX", IsCoPJourney = true, ApprenticeshipHashedId = "BBCVCVS" };
        }
    }
}

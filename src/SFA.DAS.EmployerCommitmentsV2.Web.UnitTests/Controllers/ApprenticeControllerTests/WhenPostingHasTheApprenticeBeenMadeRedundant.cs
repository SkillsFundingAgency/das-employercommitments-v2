using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    [TestFixture]
    public class WhenPostingHasTheApprenticeBeenMadeRedundant : ApprenticeControllerTestBase
    {
        [SetUp]
        public void Arrange()
        {
            _controller = new ApprenticeController(Mock.Of<IModelMapper>(),
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                Mock.Of<ICommitmentsApiClient>(), 
                Mock.Of<ILinkGenerator>(), 
                Mock.Of<ILogger<ApprenticeController>>(),
                Mock.Of<IAuthorizationService>());
        }

        [Test, MoqAutoData]
        public void AndTheUserAnswered_HasTheApprenticeBeenMadeRedundant_ThenRedirectToConfirmStopApprenticeship(MadeRedundantViewModel viewModel)
        {
            var result = _controller.HasTheApprenticeBeenMadeRedundant(viewModel);

            var redirect = result.VerifyReturnsRedirectToActionResult();
            Assert.AreEqual(redirect.ActionName, "ConfirmStop");
            Assert.AreEqual(redirect.RouteValues["AccountHashedId"], viewModel.AccountHashedId);
            Assert.AreEqual(redirect.RouteValues["ApprenticeshipHashedId"], viewModel.ApprenticeshipHashedId);
            Assert.AreEqual(redirect.RouteValues["StopMonth"], viewModel.StopMonth);
            Assert.AreEqual(redirect.RouteValues["StopYear"], viewModel.StopYear);
            Assert.AreEqual(redirect.RouteValues["IsCoPJourney"], viewModel.IsCoPJourney);
            Assert.AreEqual(redirect.RouteValues["MadeRedundant"], viewModel.MadeRedundant);
        }
    }
}

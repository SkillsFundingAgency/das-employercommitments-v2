using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    [TestFixture]
    public class WhenPostingConfirmHasValidEndDateChanges : ApprenticeControllerTestBase
    {
        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();
            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockModelMapper = new Mock<IModelMapper>();

            _controller = new ApprenticeController(_mockModelMapper.Object,
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                _mockCommitmentsApiClient.Object,
                Mock.Of<ILogger<ApprenticeController>>());

            _controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
        }

        [Test, MoqAutoData]
        public async Task AndTheApprenticeship_HasValidEndDate_ThenRedirectToApprenticeshipDetails(ConfirmHasValidEndDateViewModel request)
        {
            request.EndDateConfirmed = true;

            var result = await _controller.ConfirmHasValidEndDateChanges(request) as RedirectToActionResult;

            Assert.AreEqual("ApprenticeshipDetails", result.ActionName);

        }

        [Test, MoqAutoData]
        public async Task AndTheApprenticeship_HasNotValidEndDate_ThenRedirectToEditEndDateApprenticeship (ConfirmHasValidEndDateViewModel request)
        {
            request.EndDateConfirmed = false;

            var result = await _controller.ConfirmHasValidEndDateChanges(request) as RedirectToActionResult;

            Assert.AreEqual("EditEndDate", result.ActionName);
        }
    }
}
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using SFA.DAS.EmployerUrlHelper;
using System;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetChangeOfPartyRequestsResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenPostingSendRequestNewTrainingProviderTests
    {
        private WhenPostingSendRequestNewTrainingProviderTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenPostingSendRequestNewTrainingProviderTestsFixture();
        }

        [Test]
        public async Task VerifyRedirectsToApprenticeDetailsPage()
        {
            //Arrange
            _fixture.SetConfirm(false);

            //Act
            var result = await _fixture.SendRequestNewTrainingProvider();

            //Assert
            _fixture.VerifyRedirectsToApprenticeDetailsPage(result);
        }

        [Test]
        public async Task VerifyRedirectsToSentAction()
        {
            //Arrange
            _fixture.SetConfirm(true);

            //Act
            var result = await _fixture.SendRequestNewTrainingProvider();

            //Assert
            _fixture.VerifyRedirectsToSentAction(result);
        }

        [Test]
        public async Task VerifyCommitmentsApiCreateChangeOfPartyRequestCalled()
        {
            //Arrange
            _fixture.SetConfirm(true);

            //Act
            await _fixture.SendRequestNewTrainingProvider();

            //Assert
            _fixture.VerifyCommitmentsApiCreateChangeOfPartyRequestCalled();
        }

        [Test]
        public async Task And_NewProviderIsTheSameAsCurrentProvider_Then_RedirectToError()
        {
            _fixture.SetErrorFromCommitmentsApi();

            var actionResult = await _fixture.SendRequestNewTrainingProvider();

            _fixture.VerifyRedirectToError(actionResult);
        }

        [Test]
        public async Task RedirectingToConfirmationPage_Set_ProviderAddDetails_ToTrue()
        {
            _fixture.SetConfirm(true);

            var actionResult = (RedirectToRouteResult)await _fixture.SendRequestNewTrainingProvider();

            Assert.AreEqual(actionResult.RouteName, RouteNames.ChangeProviderRequestedConfirmation);
            Assert.AreEqual(actionResult.RouteValues[nameof(ChangeProviderRequestedConfirmationRequest.ProviderAddDetails)], true);
        }
    }

    public class WhenPostingSendRequestNewTrainingProviderTestsFixture
    {
        private readonly Mock<ICommitmentsApiClient> _commitmentsApiClient;
        private readonly Mock<ILinkGenerator> _linkGenerator;
        private ApprenticeController _controller;
        private SendNewTrainingProviderViewModel _viewModel;

        public WhenPostingSendRequestNewTrainingProviderTestsFixture()
        {
            var autoFixture = new Fixture();
            _viewModel = autoFixture.Create<SendNewTrainingProviderViewModel>();

            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _linkGenerator = new Mock<ILinkGenerator>();
            _linkGenerator.Setup(x => x.CommitmentsLink(It.IsAny<string>())).Returns<string>(s => s);

            _controller = new ApprenticeController(Mock.Of<IModelMapper>(),
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                _commitmentsApiClient.Object,
                _linkGenerator.Object,
                Mock.Of<ILogger<ApprenticeController>>(),
                Mock.Of<IAuthorizationService>());
        }

        public async Task<IActionResult> SendRequestNewTrainingProvider()
        {
            return await _controller.SendRequestNewTrainingProvider(_viewModel);
        }

        public WhenPostingSendRequestNewTrainingProviderTestsFixture SetConfirm(bool confirm)
        {
            _viewModel.Confirm = confirm;
            return this;
        }

        public WhenPostingSendRequestNewTrainingProviderTestsFixture SetErrorFromCommitmentsApi()
        {
            _commitmentsApiClient.Setup(c => c.CreateChangeOfPartyRequest(It.IsAny<long>(), It.IsAny<CreateChangeOfPartyRequestRequest>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            return this;
        }

        public void VerifyRedirectsToApprenticeDetailsPage(IActionResult result)
        {
            var redirect = (RedirectResult)result;

            redirect.WithUrl($"accounts/{_viewModel.AccountHashedId}/apprentices/manage/{_viewModel.ApprenticeshipHashedId}/details");
        }

        public void VerifyRedirectsToSentAction(IActionResult result)
        {
            var redirect = (RedirectToRouteResult)result;
            Assert.AreEqual(RouteNames.ChangeProviderRequestedConfirmation, redirect.RouteName);
        }

        public void VerifyCommitmentsApiCreateChangeOfPartyRequestCalled()
        {
            _commitmentsApiClient.Verify(p => p.CreateChangeOfPartyRequest(It.IsAny<long>(), It.IsAny<CreateChangeOfPartyRequestRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        public void VerifyRedirectToError(IActionResult actionResult)
        {
            var redirectResult = actionResult as RedirectToActionResult;

            Assert.AreEqual("Error", redirectResult.ControllerName);
            Assert.AreEqual("Error", redirectResult.ActionName);
        }
        
    }
}

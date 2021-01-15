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

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenPostingTheConfirmDetailsAndSendRequestPage 
    {
        private WhenPostingTheConfirmDetailsAndSendRequestPageTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenPostingTheConfirmDetailsAndSendRequestPageTestsFixture();
        }

        [Test]
        public async Task VerifyCommitmentsApiCreateChangeOfPartyRequestCalled()
        {
            await _fixture.ConfirmDetailsAndSendRequest();

            _fixture.VerifyCommitmentsApiCreateChangeOfPartyRequestCalled();
        }

        [Test]
        public async Task And_CommitmentsApiSuccessful_Then_RedirectToConfirmationPage()
        {
            var actionResult = await _fixture.ConfirmDetailsAndSendRequest();

            _fixture.VerifyRedirectsToSentAction(actionResult);
        }

        [Test]
        public async Task And_CommitmentsApiReturnsError_Then_RedirectToError()
        {
            _fixture.SetErrorFromCommitmentsApi();

            var actionResult = await _fixture.ConfirmDetailsAndSendRequest();

            _fixture.VerifyRedirectToError(actionResult);
        }
    }

    public class WhenPostingTheConfirmDetailsAndSendRequestPageTestsFixture
    {
        private readonly Mock<ICommitmentsApiClient> _commitmentsApiClient;
        
        private ConfirmDetailsAndSendViewModel _viewModel;

        private ApprenticeController _controller;

        public WhenPostingTheConfirmDetailsAndSendRequestPageTestsFixture()
        {
            var autoFixture = new Fixture();

            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();

            _viewModel = autoFixture.Build<ConfirmDetailsAndSendViewModel>()
                .With(x => x.NewStartDate, new DateTime(2020, 1, 1))
                .With(x => x.NewEndDate, new DateTime(2022, 1, 1))
                .Create();

            _controller = new ApprenticeController(Mock.Of<IModelMapper>(),
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                _commitmentsApiClient.Object,
                Mock.Of<ILinkGenerator>(),
                Mock.Of<ILogger<ApprenticeController>>(),
                Mock.Of<IAuthorizationService>());
        }
        public async Task<IActionResult> ConfirmDetailsAndSendRequest()
        {
            return await _controller.ConfirmDetailsAndSendRequestPage(_viewModel);
        }

        public WhenPostingTheConfirmDetailsAndSendRequestPageTestsFixture SetErrorFromCommitmentsApi()
        {
            _commitmentsApiClient.Setup(c => c.CreateChangeOfPartyRequest(It.IsAny<long>(), It.IsAny<CreateChangeOfPartyRequestRequest>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            return this;
        }

        public void VerifyCommitmentsApiCreateChangeOfPartyRequestCalled()
        {
            _commitmentsApiClient.Verify(p => p.CreateChangeOfPartyRequest(It.IsAny<long>(), It.IsAny<CreateChangeOfPartyRequestRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        public void VerifyRedirectsToSentAction(IActionResult result)
        {
            var redirect = (RedirectToRouteResult)result;
            Assert.AreEqual(RouteNames.ChangeProviderRequestedConfirmation, redirect.RouteName);

        }

        public void VerifyRedirectToError(IActionResult actionResult)
        {
            var redirectResult = actionResult as RedirectToActionResult;

            Assert.AreEqual("Error", redirectResult.ControllerName);
            Assert.AreEqual("Error", redirectResult.ActionName);
        }
    }
}

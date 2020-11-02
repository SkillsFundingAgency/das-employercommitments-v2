using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using SFA.DAS.EmployerUrlHelper;
using System.Threading.Tasks;

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
    }

    public class WhenPostingSendRequestNewTrainingProviderTestsFixture
    {
        private readonly Mock<IModelMapper> _modelMapper;
        private readonly Mock<ICommitmentsApiClient> _commitmentsApiClient;
        private readonly Mock<ILinkGenerator> _linkGenerator;
        private ApprenticeController _controller;
        private SendNewTrainingProviderViewModel _viewModel;

        public WhenPostingSendRequestNewTrainingProviderTestsFixture()
        {
            var autoFixture = new Fixture();
            _viewModel = autoFixture.Create<SendNewTrainingProviderViewModel>();

            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _modelMapper = new Mock<IModelMapper>();

            _linkGenerator = new Mock<ILinkGenerator>();
            _linkGenerator.Setup(x => x.CommitmentsLink(It.IsAny<string>())).Returns<string>(s => s);

            _controller = new ApprenticeController(Mock.Of<IModelMapper>(),
               Mock.Of<ICookieStorageService<IndexRequest>>(),
                _commitmentsApiClient.Object,
                _linkGenerator.Object);
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

        public void VerifyRedirectsToApprenticeDetailsPage(IActionResult result)
        {
            var redirect = (RedirectResult)result;
            redirect.WithUrl($"accounts/{_viewModel.AccountHashedId}/apprentices/manage/{_viewModel.ApprenticeshipHashedId}/details");
        }

        public void VerifyRedirectsToSentAction(IActionResult result)
        {
            var redirect = (RedirectToRouteResult)result;

            Assert.AreEqual(RouteNames.Sent, redirect.RouteName);
        }
    }
}

using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerUrlHelper;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenRequestingConfirmAndSendRequestPageTests
    {
        private WhenRequestingConfirmAndSendRequestPageTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenRequestingConfirmAndSendRequestPageTestsFixture();
        }



        [Test]
        public async Task ThenCorrectViewIsReturned()
        {
            var actionResult = await _fixture.ConfirmDetailsAndSendRequest();

            _fixture.VerifyViewModel(actionResult);
        }
    }

    public class WhenRequestingConfirmAndSendRequestPageTestsFixture
    {
        private readonly Mock<IModelMapper> _mockMapper;

        private readonly EmployerLedChangeOfProviderRequest _request;
        private readonly ConfirmDetailsAndSendViewModel _viewModel;

        private readonly ApprenticeController _controller;

        public WhenRequestingConfirmAndSendRequestPageTestsFixture()
        {
            var autoFixture = new Fixture();
            _request = autoFixture.Create<EmployerLedChangeOfProviderRequest>();
            _viewModel = autoFixture.Create<ConfirmDetailsAndSendViewModel>();

            _mockMapper = new Mock<IModelMapper>();
            _mockMapper.Setup(m => m.Map<ConfirmDetailsAndSendViewModel>(_request))
                .ReturnsAsync(_viewModel);

            _controller = new ApprenticeController(_mockMapper.Object,
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                Mock.Of<ICommitmentsApiClient>(),
                Mock.Of<ILinkGenerator>(),
                Mock.Of<ILogger<ApprenticeController>>(),
                Mock.Of<IAuthorizationService>());
        }

        public async Task<IActionResult> ConfirmDetailsAndSendRequest()
        {
            return await _controller.ConfirmDetailsAndSendRequestPage(_request);
        }

        public void VerifyViewModel(IActionResult actionResult)
        {
            var result = actionResult as ViewResult;
            var viewModel = result.Model;

            Assert.IsInstanceOf<ConfirmDetailsAndSendViewModel>(viewModel);

            var confirmAndSendViewModelResult = viewModel as ConfirmDetailsAndSendViewModel;

            Assert.AreEqual(_viewModel, confirmAndSendViewModelResult);
        }
    }
}

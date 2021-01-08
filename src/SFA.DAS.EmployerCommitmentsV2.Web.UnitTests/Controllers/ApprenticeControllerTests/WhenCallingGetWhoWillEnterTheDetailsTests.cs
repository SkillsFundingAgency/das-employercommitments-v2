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
    public class WhenCallingGetWhoWillEnterTheDetailsTests
    {
        WhenCallingGetWhoWillEnterTheDetailsTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenCallingGetWhoWillEnterTheDetailsTestsFixture();
        }

        [Test]
        public async Task ThenTheCorrectViewIsReturned()
        {
            var result = await _fixture.WhoWillEnterTheDetails();

            _fixture.VerifyViewModel(result as ViewResult);
        }
    }

    public class WhenCallingGetWhoWillEnterTheDetailsTestsFixture
    {
        private readonly WhoWillEnterTheDetailsRequest _request;
        private readonly WhoWillEnterTheDetailsViewModel _viewModel;

        private readonly Mock<IModelMapper> _mockMapper;

        private readonly ApprenticeController _controller;

        public WhenCallingGetWhoWillEnterTheDetailsTestsFixture()
        {
            var autoFixture = new Fixture();

            _request = autoFixture.Create<WhoWillEnterTheDetailsRequest>();
            _viewModel = autoFixture.Create<WhoWillEnterTheDetailsViewModel>();

            _mockMapper = new Mock<IModelMapper>();
            _mockMapper.Setup(m => m.Map<WhoWillEnterTheDetailsViewModel>(_request))
                .ReturnsAsync(_viewModel);

            _controller = new ApprenticeController(_mockMapper.Object,
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                Mock.Of<ICommitmentsApiClient>(),
                Mock.Of<ILinkGenerator>(),
                Mock.Of<ILogger<ApprenticeController>>(),
                Mock.Of<IAuthorizationService>());
        }

        public async Task<IActionResult> WhoWillEnterTheDetails()
        {
            return await _controller.WhoWillEnterTheDetails(_request);
        }

        public void VerifyViewModel(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as WhoWillEnterTheDetailsViewModel;

            Assert.IsInstanceOf<WhoWillEnterTheDetailsViewModel>(viewModel);
            Assert.AreEqual(_viewModel, viewModel);
        }
    }
}

using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenCallingGetSendRequestNewTrainingProviderTests
    {
        WhenCallingGetSendRequestNewTrainingProviderTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenCallingGetSendRequestNewTrainingProviderTestsFixture();
        }

        [Test]
        public async Task Then_The_View_Is_Returned()
        {
            var actionResult = await _fixture.SendRequestNewTrainingProvider();

            _fixture.VerifyViewModel(actionResult);
        }
    }

    public class WhenCallingGetSendRequestNewTrainingProviderTestsFixture
    {
        private readonly Mock<IModelMapper> _modelMapper;
        private ApprenticeController _controller;
        private SendNewTrainingProviderViewModel _expectedViewModel;
        private SendNewTrainingProviderRequest _request;

        public WhenCallingGetSendRequestNewTrainingProviderTestsFixture()
        {
            var autoFixture = new Fixture();
            _expectedViewModel = autoFixture.Create<SendNewTrainingProviderViewModel>();
            _request = autoFixture.Create<SendNewTrainingProviderRequest>();


            _modelMapper = new Mock<IModelMapper>();
            _modelMapper
               .Setup(mapper => mapper.Map<SendNewTrainingProviderViewModel>(_request))
               .ReturnsAsync(_expectedViewModel);

            _controller = new ApprenticeController(_modelMapper.Object,
               Mock.Of<ICookieStorageService<IndexRequest>>(),
               Mock.Of<ICommitmentsApiClient>(),
               Mock.Of<ILinkGenerator>(), 
               Mock.Of<ILogger<ApprenticeController>>());
        }

        public async Task<IActionResult> SendRequestNewTrainingProvider()
        {
            return await _controller.SendRequestNewTrainingProvider(_request);
        }

        public void VerifyViewModel(IActionResult actionResult)
        {
            var result = actionResult as ViewResult;
            var viewModel = result.Model;

            Assert.IsInstanceOf<SendNewTrainingProviderViewModel>(viewModel);
            var sendNewTrainingProviderViewModel = (SendNewTrainingProviderViewModel)viewModel;

            Assert.AreEqual(_expectedViewModel, sendNewTrainingProviderViewModel);
        }
    }
}

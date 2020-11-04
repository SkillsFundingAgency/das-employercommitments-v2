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
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenPostingEnterNewTrainingProvider
    {
        private WhenPostingEnterNewTrainingProviderFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenPostingEnterNewTrainingProviderFixture();
        }

        [Test]
        public async Task ThenRedirectToSendRequestPage()
        {
            var result = await _fixture.EnterNewTrainingProvider();

            _fixture.VerifyRedirectsToSendNewTrainingProviderRequest(result);
        } 
    }

    public class WhenPostingEnterNewTrainingProviderFixture
    {
        private readonly Mock<IModelMapper> _mockMapper;
        private readonly Mock<ILinkGenerator> _mockLinkGenerator;
        private readonly EnterNewTrainingProviderViewModel _viewModel;

        private readonly ApprenticeController _controller;

        public WhenPostingEnterNewTrainingProviderFixture()
        {
            var autoFixture = new Fixture();

            _viewModel = autoFixture.Create<EnterNewTrainingProviderViewModel>();

            _mockMapper = new Mock<IModelMapper>();
            _mockMapper.Setup(m => m.Map<SendNewTrainingProviderRequest>(_viewModel))
                .ReturnsAsync(new SendNewTrainingProviderRequest { AccountHashedId = _viewModel.AccountHashedId, ApprenticeshipHashedId = _viewModel.ApprenticeshipHashedId, Ukprn = _viewModel.Ukprn });

            _mockLinkGenerator = new Mock<ILinkGenerator>();
            _mockLinkGenerator.Setup(x => x.CommitmentsLink(It.IsAny<string>())).Returns<string>(s => s);

            _controller = new ApprenticeController(_mockMapper.Object, Mock.Of<ICookieStorageService<IndexRequest>>(), Mock.Of<ICommitmentsApiClient>(), _mockLinkGenerator.Object);
        
        }

        public async Task<IActionResult> EnterNewTrainingProvider()
        {
            return await _controller.EnterNewTrainingProvider(_viewModel);
        }

        public void VerifyRedirectsToSendNewTrainingProviderRequest(IActionResult result)
        {
            var redirect = (RedirectToRouteResult)result;
           
            Assert.AreEqual(RouteNames.SendRequestNewTrainingProvider, redirect.RouteName);
        }

    }
}

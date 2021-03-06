﻿using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
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

            _fixture.VerifyViewModel(actionResult as ViewResult) ;
        }
    }

    public class WhenRequestingConfirmAndSendRequestPageTestsFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly ChangeOfProviderRequest _request;
        private readonly ConfirmDetailsAndSendViewModel _viewModel;

        public WhenRequestingConfirmAndSendRequestPageTestsFixture() : base()
        {
            _request = _autoFixture.Create<ChangeOfProviderRequest>();
            _viewModel = _autoFixture.Create<ConfirmDetailsAndSendViewModel>();

            
            _mockMapper.Setup(m => m.Map<ConfirmDetailsAndSendViewModel>(_request))
                .ReturnsAsync(_viewModel);
        }

        public async Task<IActionResult> ConfirmDetailsAndSendRequest()
        {
            return await _controller.ConfirmDetailsAndSendRequestPage(_request);
        }

        public void VerifyViewModel(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as ConfirmDetailsAndSendViewModel;

            Assert.IsInstanceOf<ConfirmDetailsAndSendViewModel>(viewModel);
            Assert.AreEqual(_viewModel, viewModel);
        }
    }
}

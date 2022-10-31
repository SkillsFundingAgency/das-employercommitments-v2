﻿using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenRequestingConfirmApprenticeshipHasValidEndDatePage
    {
        private WhenRequestingConfirmApprenticeshipHasValidEndDatePageFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenRequestingConfirmApprenticeshipHasValidEndDatePageFixture();
        }

        [Test]
        public async Task WhenRequesting_ConfirmApprenticeshipHasValidEndDate_ThenConfirmHasValidEndDateRequestViewModelIsPassedToTheView()
        {
            var actionResult = await _fixture.ConfirmHasValidEndDate();

            _fixture.VerifyViewModel(actionResult as ViewResult);
        }
    }
    public class WhenRequestingConfirmApprenticeshipHasValidEndDatePageFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly ConfirmHasValidEndDateRequest _request;
        private readonly ConfirmHasValidEndDateViewModel _viewModel;
        public WhenRequestingConfirmApprenticeshipHasValidEndDatePageFixture() : base()
        {
            _request = _autoFixture.Create<ConfirmHasValidEndDateRequest>();
            _viewModel = _autoFixture.Create<ConfirmHasValidEndDateViewModel>();


            _mockMapper.Setup(m => m.Map<ConfirmHasValidEndDateViewModel>(_request))
                .ReturnsAsync(_viewModel);
        }

        public async Task<IActionResult> ConfirmHasValidEndDate()
        {
            return await _controller.ConfirmHasValidEndDate(_request);
        }

        public void VerifyViewModel(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as ConfirmHasValidEndDateViewModel;

            Assert.IsInstanceOf<ConfirmHasValidEndDateViewModel>(viewModel);
            Assert.AreEqual(_viewModel, viewModel);
        }
    }
}
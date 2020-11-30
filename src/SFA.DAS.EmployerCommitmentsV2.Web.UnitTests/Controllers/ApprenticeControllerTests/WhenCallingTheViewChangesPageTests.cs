using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerUrlHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenCallingTheViewChangesPageTests
    {
        WhenCallingTheViewChangesPageTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenCallingTheViewChangesPageTestsFixture();
        }

        [Test]
        public async Task ThenViewIsReturned()
        {
            var actionResult = await _fixture.ViewChanges();

            _fixture.VerifyViewModel(actionResult);
        }
    }

    public class WhenCallingTheViewChangesPageTestsFixture
    {
        private Mock<IModelMapper> _mockMapper;

        private ViewChangesRequest _request;
        private ViewChangesViewModel _viewModel;

        private ApprenticeController _controller;

        public WhenCallingTheViewChangesPageTestsFixture()
        {
            var autoFixture = new Fixture();
            _request = autoFixture.Create<ViewChangesRequest>();
            _viewModel = autoFixture.Create<ViewChangesViewModel>();

            _mockMapper = new Mock<IModelMapper>();
            _mockMapper.Setup(m => m.Map<ViewChangesViewModel>(_request))
                .ReturnsAsync(_viewModel);

            _controller = new ApprenticeController(_mockMapper.Object,
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                Mock.Of<ICommitmentsApiClient>(),
                Mock.Of<ILinkGenerator>(),
                Mock.Of<ILogger<ApprenticeController>>());
        }

        public async Task<IActionResult> ViewChanges()
        {
            return await _controller.ViewChanges(_request);
        }

        public void VerifyViewModel(IActionResult actionResult)
        {
            var result = actionResult as ViewResult;
            var viewModel = result.Model;

            Assert.IsInstanceOf<ViewChangesViewModel>(viewModel);

            var viewChangesViewModelResult = viewModel as ViewChangesViewModel;

            Assert.AreEqual(_viewModel, viewChangesViewModelResult);
        }
    }
}

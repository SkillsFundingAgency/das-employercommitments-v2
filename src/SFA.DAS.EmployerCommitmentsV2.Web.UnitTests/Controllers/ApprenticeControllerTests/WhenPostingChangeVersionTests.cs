using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerUrlHelper;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenPostingChangeVersionTests : ApprenticeControllerTestBase
    {
        private ChangeVersionViewModel _viewModel;
        private EditApprenticeshipRequestViewModel _editRequestViewModel;

        [SetUp]
        public void Arrange()
        {
            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockModelMapper = new Mock<IModelMapper>();
            
            var autoFixture = new Fixture();

            _viewModel = autoFixture.Create<ChangeVersionViewModel>();
            
            var baseDate = DateTime.Now;
            var startDate = new MonthYearModel(baseDate.ToString("MMyyyy"));
            var endDate = new MonthYearModel(baseDate.AddYears(2).ToString("MMyyyy"));
            var dateOfBirth = new MonthYearModel(baseDate.AddYears(-18).ToString("MMyyyy"));

            _editRequestViewModel = autoFixture.Build<EditApprenticeshipRequestViewModel>()
                    .With(x => x.StartDate, startDate)
                    .With(x => x.EndDate, endDate)
                    .With(x => x.DateOfBirth, dateOfBirth)
                .Create();

            _mockModelMapper.Setup(m => m.Map<EditApprenticeshipRequestViewModel>(It.IsAny<ChangeVersionViewModel>()))
                .ReturnsAsync(_editRequestViewModel);

            _controller = new ApprenticeController(_mockModelMapper.Object, 
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                _mockCommitmentsApiClient.Object,
                Mock.Of<ILinkGenerator>(),
                Mock.Of<ILogger<ApprenticeController>>());

            _controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
        }

        [Test]
        public async Task Then_VerifyMapperIsCalled()
        {
            await _controller.ChangeVersion(_viewModel);

            _mockModelMapper.Verify(m => m.Map<EditApprenticeshipRequestViewModel>(It.IsAny<ChangeVersionViewModel>()), Times.Once());
        }

        [Test]
        public async Task And_VersionHasOptions_Then_VerifyRedirectToChangeOption()
        {
            _editRequestViewModel.HasOptions = true;

            var result = await _controller.ChangeVersion(_viewModel);

            result.VerifyReturnsRedirectToActionResult().WithActionName("ChangeOption");
        }

        [Test]
        public async Task And_VersionHasNoOptions_Then_VerifyRedirectToConfirmEditApprenticeship()
        {
            _editRequestViewModel.HasOptions = false;

            var result = await _controller.ChangeVersion(_viewModel);

            result.VerifyReturnsRedirectToActionResult().WithActionName("ConfirmEditApprenticeship");
        }
    }
}

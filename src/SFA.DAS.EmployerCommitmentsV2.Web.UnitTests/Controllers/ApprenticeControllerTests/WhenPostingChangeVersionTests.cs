using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenPostingChangeVersionTests : ApprenticeControllerTestBase
{
    private ChangeVersionViewModel _viewModel;
    private EditApprenticeshipRequestViewModel _editRequestViewModel;

    [SetUp]
    public void Arrange()
    {
        MockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        MockModelMapper = new Mock<IModelMapper>();
        CacheStorageService = new Mock<ICacheStorageService>();
        ApprovalsApiClient = new Mock<IApprovalsApiClient>();

        var autoFixture = new Fixture();

        _viewModel = autoFixture.Create<ChangeVersionViewModel>();
            
        var baseDate = DateTime.Now;
        var startDate = new MonthYearModel(baseDate.ToString("MMyyyy"));
        var endDate = new MonthYearModel(baseDate.AddYears(2).ToString("MMyyyy"));
        var employmentEndDate = new MonthYearModel(baseDate.AddYears(1).ToString("MMyyyy"));
        var dateOfBirth = new MonthYearModel(baseDate.AddYears(-18).ToString("MMyyyy"));

        _editRequestViewModel = autoFixture.Build<EditApprenticeshipRequestViewModel>()
            .With(x => x.StartDate, startDate)
            .With(x => x.EndDate, endDate)
            .With(x => x.EmploymentEndDate, employmentEndDate)
            .With(x => x.DateOfBirth, dateOfBirth)
            .Create();

        MockModelMapper.Setup(m => m.Map<EditApprenticeshipRequestViewModel>(It.IsAny<ChangeVersionViewModel>()))
            .ReturnsAsync(_editRequestViewModel);

        Controller = new ApprenticeController(MockModelMapper.Object, 
            Mock.Of<Interfaces.ICookieStorageService<IndexRequest>>(),
            MockCommitmentsApiClient.Object,
            CacheStorageService.Object,
            Mock.Of<ILogger<ApprenticeController>>(),
            ApprovalsApiClient.Object);

        Controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
    }

    [Test]
    public async Task Then_VerifyMapperIsCalled()
    {
        await Controller.ChangeVersion(_viewModel);

        MockModelMapper.Verify(m => m.Map<EditApprenticeshipRequestViewModel>(_viewModel), Times.Once());
    }
    
    [Test]
    public async Task Then_NewCacheKeyIsAssigned()
    {
        _viewModel.CacheKey = null;

        MockModelMapper
            .Setup(m => m.Map<EditApprenticeshipRequestViewModel>(
                It.Is<ChangeVersionViewModel>(x => x.CacheKey != null)))
            .ReturnsAsync(new EditApprenticeshipRequestViewModel());

        var result = await Controller.ChangeVersion(_viewModel);

        var redirectResult = result.Should().BeOfType<RedirectToActionResult>();
        var cacheKey = redirectResult.Subject.RouteValues["cacheKey"].ToString();
        cacheKey.Should().NotBeNull();
        var isGuid = Guid.TryParse(cacheKey, out _);
        isGuid.Should().BeTrue();
    }

    [Test]
    public async Task And_VersionHasOptions_Then_VerifyRedirectToChangeOption()
    {
        _editRequestViewModel.HasOptions = true;

        var result = await Controller.ChangeVersion(_viewModel);

        result.VerifyReturnsRedirectToActionResult().WithActionName("ChangeOption");
    }

    [Test]
    public async Task And_VersionHasNoOptions_Then_VerifyRedirectToConfirmEditApprenticeship()
    {
        _editRequestViewModel.HasOptions = false;

        var result = await Controller.ChangeVersion(_viewModel);

        result.VerifyReturnsRedirectToActionResult().WithActionName("ConfirmEditApprenticeship");
    }
}
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenPostingEditApprenticeshipDetails
{
    private Fixture _autoFixture;
    private WhenPostingEditApprenticeshipDetailsFixture _fixture;
    private EditApprenticeshipRequestViewModel _viewModel;

    private GetApprenticeshipResponse _apprenticeshipResponse;
    private GetTrainingProgrammeResponse _standardVersionResponse;
    private GetTrainingProgrammeResponse _frameworkResponse;
    private ValidateEditApprenticeshipResponse _validateEditApprenticeshipResponse;

    [SetUp]
    public void Arrange()
    {
        _autoFixture = new Fixture();

        _fixture = new WhenPostingEditApprenticeshipDetailsFixture();

        _apprenticeshipResponse = _autoFixture.Build<GetApprenticeshipResponse>()
            .With(x => x.CourseCode, _autoFixture.Create<int>().ToString())
            .Create();

        _standardVersionResponse = _autoFixture.Build<GetTrainingProgrammeResponse>()
            .With(x => x.TrainingProgramme, _autoFixture.Build<TrainingProgramme>()
                .With(x => x.Version, "1.0")
                .With(x => x.Options, new List<string>())
                .Create())
            .Create();

        _frameworkResponse = _autoFixture.Create<GetTrainingProgrammeResponse>();
        _frameworkResponse.TrainingProgramme.Version = null;

        _validateEditApprenticeshipResponse = _autoFixture.Build<ValidateEditApprenticeshipResponse>()
            .With(x => x.HasOptions, false)
            .With(x => x.Version, "1.0")
            .With(x => x.CourseOrStartDateChange, false)
            .Create();

        _viewModel = _autoFixture.Build<EditApprenticeshipRequestViewModel>()
            .Without(x => x.StartDate)
            .Without(x => x.EndDate)
            .Without(x => x.EmploymentEndDate)
            .Without(x => x.DateOfBirth)
            .With(x => x.CourseCode, _apprenticeshipResponse.CourseCode)
            .Create();

        _viewModel.StartDate = new MonthYearModel(_apprenticeshipResponse.StartDate.Value.ToString("MMyyyy"));

        _fixture.SetUpGetApprenticeship(_apprenticeshipResponse);
        _fixture.SetUpEditApprenticeship(_validateEditApprenticeshipResponse);
    }

    [Test]
    public async Task And_NewStandardSelected_Then_EditApprenticeshipIsCalled()
    {
        _viewModel.CourseCode = _autoFixture.Create<int>().ToString();

        await _fixture.EditApprenticeship(_viewModel);

        _fixture.VerifyEditApprenticeshipIsCalled();
    }

    [Test]
    public async Task And_StandardIsSelected_And_StartDateMovedForward_Then_EditApprenticeshipIsCalled()
    {
        _viewModel.StartDate = new MonthYearModel(_viewModel.StartDate.Date.Value.AddMonths(1).ToString("MMyyy"));

        await _fixture.EditApprenticeship(_viewModel);

        _fixture.VerifyEditApprenticeshipIsCalled();
    }

    [Test]
    public async Task And_StandardNotChanged_And_StartDateNotMovedForward_Then_EditApprenticeshipIsCalled()
    {
        await _fixture.EditApprenticeship(_viewModel);

        _fixture.VerifyEditApprenticeshipIsCalled();
    }

    [Test]
    public async Task And_StartDateIsMovedForward_And_FrameworkNotChanged_Then_EditApprenticeshipIsCalled()
    {
        _viewModel.StartDate = new MonthYearModel(_viewModel.StartDate.Date.Value.AddMonths(1).ToString("MMyyy"));
        _viewModel.CourseCode = "1-2-3";
        _apprenticeshipResponse.CourseCode = "1-2-3";

        await _fixture.EditApprenticeship(_viewModel);

        _fixture.VerifyEditApprenticeshipIsCalled();
    }

    [Test]
    public async Task And_FrameworkIsChanged_Then_EditApprenticeshipIsCalled()
    {
        _viewModel.CourseCode = "4-5-6";
        _apprenticeshipResponse.CourseCode = "1-2-3";

        await _fixture.EditApprenticeship(_viewModel);

        _fixture.VerifyEditApprenticeshipIsCalled();
    }

    [Test]
    public async Task And_FrameworkNotChanged_And_StartDateNotMovedForward_Then_EditApprenticeshipIsCalled()
    {
        _viewModel.CourseCode = "1-2-3";
        _apprenticeshipResponse.CourseCode = "1-2-3";

        await _fixture.EditApprenticeship(_viewModel);

        _fixture.VerifyEditApprenticeshipIsCalled();
    }

    [Test]
    public async Task And_StandardHasOptions_Then_RedirectToChangeOption()
    {
        _validateEditApprenticeshipResponse.HasOptions = true;
        _fixture.SetUpEditApprenticeship(_validateEditApprenticeshipResponse);

        var result = await _fixture.EditApprenticeship(_viewModel);

        WhenPostingEditApprenticeshipDetailsFixture.VerifyRedirectToChangeOption(result as RedirectToActionResult);
    }

    [Test]
    public async Task VerifyValidationApiIsCalled()
    {
        await _fixture.EditApprenticeship(_viewModel);
        _fixture.VerifyValidationApiIsCalled();
    }

    [Test]
    public async Task VerifyMapperIsCalled()
    {
        await _fixture.EditApprenticeship(_viewModel);
        _fixture.VerifyMapperIsCalled();
    }

    [Test]
    public async Task AndSelectCourseIsToBeChangedThenTheUserIsRedirectedToSelectCoursePage()
    {
        var result = await _fixture.EditChangingCourse(_viewModel);
        WhenPostingEditApprenticeshipDetailsFixture.VerifyRedirectedTo(result, nameof(ApprenticeController.SelectCourseForEdit));
        WhenPostingEditApprenticeshipDetailsFixture.VerifyRouteValue(result, "AccountHashedId", _viewModel.AccountHashedId);
        WhenPostingEditApprenticeshipDetailsFixture.VerifyRouteValue(result, "ApprenticeshipHashedId", _viewModel.HashedApprenticeshipId);
    }

    [Test]
    public async Task AndSelectDeliveryModelIsToBeChangedThenTheUserIsRedirectedToSelectDeliveryModelPage()
    {
        var result = await _fixture.EditChangingDeliveryModel(_viewModel);
        WhenPostingEditApprenticeshipDetailsFixture.VerifyRedirectedTo(result, nameof(ApprenticeController.SelectDeliveryModelForEdit));
    }

    [Test]
    public void When_ApiReturnsNull_Then_ThrowsException()
    {
        _fixture.SetUpEditApprenticeship(null);
        Func<Task> act = async () => await _fixture.EditApprenticeship(_viewModel);
        act.Should().ThrowAsync<NullReferenceException>();
    }

    [Test]
    public void When_ApiThrowsException_Then_PropagatesException()
    {
        _fixture.SetUpEditApprenticeshipToThrow();
        Func<Task> act = async () => await _fixture.EditApprenticeship(_viewModel);
        act.Should().ThrowAsync<Exception>();
    }

    [Test]
    public async Task When_ApiReturnsResponse_Then_ViewModelPropertiesAreSet()
    {
        var response = new ValidateEditApprenticeshipResponse
        {
            HasOptions = true,
            Version = "2.0",
            CourseOrStartDateChange = false
        };
        _fixture.SetUpEditApprenticeship(response);
        await _fixture.EditApprenticeship(_viewModel);
        _viewModel.HasOptions.Should().Be(response.HasOptions);
        _viewModel.Version.Should().Be(response.Version);
        _viewModel.Option.Should().Be(_viewModel.Option); // unchanged
    }

    [Test]
    public async Task When_ApiReturnsNullVersion_Then_ViewModelVersionIsNull()
    {
        var response = new ValidateEditApprenticeshipResponse
        {
            HasOptions = false,
            Version = null,
            CourseOrStartDateChange = false
        };
        _fixture.SetUpEditApprenticeship(response);
        await _fixture.EditApprenticeship(_viewModel);
        _viewModel.Version.Should().BeNull();
    }

    [Test]
    public async Task When_CourseOrStartDateChangeTrue_And_OptionAlreadyNull_Then_OptionRemainsNull()
    {
        var response = new ValidateEditApprenticeshipResponse
        {
            HasOptions = false,
            Version = "1.0",
            CourseOrStartDateChange = true
        };
        _fixture.SetUpEditApprenticeship(response);
        _viewModel.Option = null;
        await _fixture.EditApprenticeship(_viewModel);
        _viewModel.Option.Should().BeNull();
    }

    [Test]
    public async Task When_CacheKeyIsMissing_Then_StillRedirectsAndCaches()
    {
        _viewModel.CacheKey = null;
        var result = await _fixture.EditApprenticeship(_viewModel);
        result.Should().BeOfType<RedirectToActionResult>();
    }
}

public class WhenPostingEditApprenticeshipDetailsFixture : ApprenticeControllerTestFixtureBase
{
    public WhenPostingEditApprenticeshipDetailsFixture()
    {
        Controller.TempData = new TempDataDictionary(Mock.Of<HttpContext>(), Mock.Of<ITempDataProvider>());
    }

    public async Task<IActionResult> EditApprenticeship(EditApprenticeshipRequestViewModel viewModel)
    {
        return await Controller.EditApprenticeship(null, null, viewModel);
    }

    public async Task<IActionResult> EditChangingCourse(EditApprenticeshipRequestViewModel viewModel)
    {
        return await Controller.EditApprenticeship("Edit", null, viewModel);
    }

    public async Task<IActionResult> EditChangingDeliveryModel(EditApprenticeshipRequestViewModel viewModel)
    {
        return await Controller.EditApprenticeship(null, "Edit", viewModel);
    }

    public void SetUpGetApprenticeship(GetApprenticeshipResponse response)
    {
        MockCommitmentsApiClient.Setup(c => c.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
    }

    public void SetUpEditApprenticeship(ValidateEditApprenticeshipResponse response)
    {
        ApprovalsApiClientMock.Setup(c => c.EditApprenticeship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ValidateEditApprenticeshipRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
    }

    public void SetUpEditApprenticeshipToThrow()
    {
        ApprovalsApiClientMock.Setup(c => c.EditApprenticeship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ValidateEditApprenticeshipRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("API error"));
    }

    public void VerifyEditApprenticeshipIsCalled()
    {
        ApprovalsApiClientMock.Verify(x => x.EditApprenticeship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ValidateEditApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    public void VerifyValidationApiIsCalled()
    {
        ApprovalsApiClientMock.Verify(x => x.EditApprenticeship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ValidateEditApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    public void VerifyMapperIsCalled()
    {
        MockMapper.Verify(x => x.Map<ValidateEditApprenticeshipRequest>(It.IsAny<EditApprenticeshipRequestViewModel>()), Times.Once());
    }

    public static void VerifyRedirectToChangeOption(RedirectToActionResult result)
    {
        result.ActionName.Should().Be("ChangeOption");
    }

    public static void VerifyRedirectedTo(IActionResult actionResult, string actionName)
    {
        actionResult.VerifyReturnsRedirectToActionResult().WithActionName(actionName);
    }

    public static void VerifyRouteValue(IActionResult actionResult, string name, string value)
    {
        actionResult.VerifyRouteValue(name, value);
    }
}
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenPostingEditApprenticeshipDetails
    {
        private Fixture _autoFixture;
        private WhenPostingEditApprenticeshipDetailsFixture _fixture;
        private EditApprenticeshipRequestViewModel _viewModel;

        private GetApprenticeshipResponse _apprenticeshipResponse;
        private GetTrainingProgrammeResponse _standardVersionResponse;
        private GetTrainingProgrammeResponse _frameworkResponse;

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

            _viewModel = _autoFixture.Build<EditApprenticeshipRequestViewModel>()
                .Without(x => x.StartDate)
                .Without(x => x.EndDate)
                .Without(x => x.DateOfBirth)
                .With(x => x.CourseCode, _apprenticeshipResponse.CourseCode)
                .Create();

            _viewModel.StartDate = new MonthYearModel(_apprenticeshipResponse.StartDate.ToString("MMyyyy"));
            
            _fixture.SetUpGetApprenticeship(_apprenticeshipResponse);
        }   

        [Test]
        public async Task And_NewStandardSelected_Then_GetCalculatedVersion()
        {
            _viewModel.CourseCode = _autoFixture.Create<int>().ToString();
            _fixture.SetUpGetCalculatedTrainingProgrammeVersion(_viewModel, _standardVersionResponse);

            await _fixture.EditApprenticeship(_viewModel);

            _fixture.VerifyGetCalculatedTrainingProgrameVersionIsCalled();
        }

        [Test]
        public async Task And_StandardIsSelected_And_StartDateMovedForward_Then_GetCalculatedVersion()
        {
            _viewModel.StartDate = new MonthYearModel(_viewModel.StartDate.Date.Value.AddMonths(1).ToString("MMyyy"));
            _fixture.SetUpGetCalculatedTrainingProgrammeVersion(_viewModel, _standardVersionResponse);

            await _fixture.EditApprenticeship(_viewModel);

            _fixture.VerifyGetCalculatedTrainingProgrameVersionIsCalled();
        }
            
        [Test]
        public async Task And_StandardNotChanged_And_StartDateNotMovedForward_Then_NeitherGetTrainingProgrammeMethodCalled()
        {
            await _fixture.EditApprenticeship(_viewModel);

            _fixture.VerifyGetCalculatedTrainingProgrameVersionIsNotCalled();
            _fixture.VerifyGetTrainingProgrameIsNotCalled();
        }

        [Test]
        public async Task And_StartDateIsMovedForward_And_FrameworkNotChanged_Then_GetTrainingProgramme()
        {
            _viewModel.StartDate = new MonthYearModel(_viewModel.StartDate.Date.Value.AddMonths(1).ToString("MMyyy"));
            _viewModel.CourseCode = "1-2-3";
            _apprenticeshipResponse.CourseCode = "1-2-3";
            _fixture.SetUpGetTrainingProgramme(_viewModel, _frameworkResponse);

            await _fixture.EditApprenticeship(_viewModel);

            _fixture.VerifyGetTrainingProgrameIsCalled();
        }

        [Test]
        public async Task And_FrameworkIsChanged_Then_GetTrainingProgramme()
        {
            _viewModel.CourseCode = "4-5-6";
            _apprenticeshipResponse.CourseCode = "1-2-3";
            _fixture.SetUpGetTrainingProgramme(_viewModel, _frameworkResponse);

            await _fixture.EditApprenticeship(_viewModel);

            _fixture.VerifyGetTrainingProgrameIsCalled();
        }

        [Test]
        public async Task And_FrameworkNotChanged_And_StartDateNotMovedForward_Then_GetTrainingProgrammeIsNotCalled()
        {
            _viewModel.CourseCode = "1-2-3";
            _apprenticeshipResponse.CourseCode = "1-2-3";

            await _fixture.EditApprenticeship(_viewModel);

            _fixture.VerifyGetTrainingProgrameIsNotCalled();
        }

        [Test]
        public async Task And_StandardHasOptions_Then_RedirectToChangeOption()
        {
            _standardVersionResponse.TrainingProgramme.Options = _autoFixture.Create<List<string>>();
            _viewModel.HasOptions = true;

            var result = await _fixture.EditApprenticeship(_viewModel);

            _fixture.VerifyRedirectToChangeOption(result as RedirectToActionResult);
        }

        [Test]
        public async Task VerifyValidationApiIsCalled()
        {
            var result = await _fixture.EditApprenticeship(_viewModel);
            _fixture.VerifyValidationApiIsCalled();
        }

        [Test]
        public async Task VerifyMapperIsCalled()
        {
            await _fixture.EditApprenticeship(_viewModel);
            _fixture.VerifyMapperIsCalled();
        }
    }

    public class WhenPostingEditApprenticeshipDetailsFixture : ApprenticeControllerTestFixtureBase
    {
        public WhenPostingEditApprenticeshipDetailsFixture() : base () 
        {
            _controller.TempData = new TempDataDictionary( Mock.Of<HttpContext>(), Mock.Of<ITempDataProvider>());
        }

        public async Task<IActionResult> EditApprenticeship(EditApprenticeshipRequestViewModel viewModel)
        {
            return await _controller.EditApprenticeship(null, null, viewModel);
        }     

        public void SetUpGetApprenticeship(GetApprenticeshipResponse response)
        {
            _mockCommitmentsApiClient.Setup(c => c.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
        }

        public void SetUpGetCalculatedTrainingProgrammeVersion(EditApprenticeshipRequestViewModel viewModel, GetTrainingProgrammeResponse response)
        {
            _mockCommitmentsApiClient.Setup(c => c.GetCalculatedTrainingProgrammeVersion(int.Parse(viewModel.CourseCode), viewModel.StartDate.Date.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
        }

        public void SetUpGetTrainingProgramme(EditApprenticeshipRequestViewModel viewModel, GetTrainingProgrammeResponse response)
        {
            _mockCommitmentsApiClient.Setup(c => c.GetTrainingProgramme(viewModel.CourseCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
        }

        public void VerifyGetCalculatedTrainingProgrameVersionIsCalled()
        {
            _mockCommitmentsApiClient.Verify(x => x.GetCalculatedTrainingProgrammeVersion(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        public void VerifyGetTrainingProgrameIsCalled()
        {
            _mockCommitmentsApiClient.Verify(x => x.GetTrainingProgramme(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        public void VerifyGetCalculatedTrainingProgrameVersionIsNotCalled()
        {
            _mockCommitmentsApiClient.Verify(x => x.GetCalculatedTrainingProgrammeVersion(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        public void VerifyGetTrainingProgrameIsNotCalled()
        {
            _mockCommitmentsApiClient.Verify(x => x.GetTrainingProgramme(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        public void VerifyValidationApiIsCalled()
        {
            _mockCommitmentsApiClient.Verify(x => x.ValidateApprenticeshipForEdit(It.IsAny<ValidateApprenticeshipForEditRequest>(), CancellationToken.None), Times.Once());
        }

        public void VerifyMapperIsCalled()
        {
            _mockMapper.Verify(x => x.Map<ValidateApprenticeshipForEditRequest>(It.IsAny<EditApprenticeshipRequestViewModel>()), Times.Once());
        }

        public void VerifyRedirectToChangeOption(RedirectToActionResult result)
        {
            result.ActionName.Should().Be("ChangeOption");
        }
    }
}

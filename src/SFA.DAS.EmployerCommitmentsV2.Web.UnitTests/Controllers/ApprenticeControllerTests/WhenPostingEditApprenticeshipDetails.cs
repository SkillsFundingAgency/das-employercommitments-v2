using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenPostingEditApprenticeshipDetails
    {
        private Fixture _autoFixture;
        private WhenPostingEditApprenticeshipDetailsFixture _fixture;
        private EditApprenticeshipRequestViewModel _viewModel;

        [SetUp]
        public void Arrange()
        {
            _autoFixture = new Fixture();
            _fixture = new WhenPostingEditApprenticeshipDetailsFixture();
            _viewModel = new EditApprenticeshipRequestViewModel();
        }   

        [Test]
        public async Task And_StandardSelected_Then_GetCalculatedVersion()
        {
            _viewModel.CourseCode = "1";

            await _fixture.EditApprenticeship(_viewModel);

            _fixture.VerifyGetCalculatedTrainingProgrameVersionIsCalled();
        }

        [Test]
        public async Task And_FrameworkSelected_Then_GetCalculatedVersion()
        {
            _viewModel.CourseCode = "1-2-3";

            await _fixture.EditApprenticeship(_viewModel);

            _fixture.VerifyGetCalculatedTrainingProgrameVersionIsCalled();
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
            return await _controller.EditApprenticeship(viewModel);
        }     

        public void VerifyGetCalculatedTrainingProgrameVersionIsCalled()
        {
            _mockCommitmentsApiClient.Verify(x => x.GetCalculatedTrainingProgrammeVersion(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        public void VerifyGetTrainingProgrameIsCalled()
        {
            _mockCommitmentsApiClient.Verify(x => x.GetTrainingProgramme(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        public void VerifyValidationApiIsCalled()
        {
            _mockCommitmentsApiClient.Verify(x => x.ValidateApprenticeshipForEdit(It.IsAny<ValidateApprenticeshipForEditRequest>(), CancellationToken.None), Times.Once());
        }

        public void VerifyMapperIsCalled()
        {
            _mockMapper.Verify(x => x.Map<ValidateApprenticeshipForEditRequest>(It.IsAny<EditApprenticeshipRequestViewModel>()), Times.Once());
        }
    }
}

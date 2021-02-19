using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenPostingChangeStatus : ApprenticeControllerTestBase
    { 
        [SetUp]
        public void Arrange()
        {
            _mockModelMapper = new Mock<IModelMapper>();
            _mockCookieStorageService = new Mock<ICookieStorageService<IndexRequest>>();
            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockLinkGenerator = new Mock<ILinkGenerator>();

            _controller = new ApprenticeController(_mockModelMapper.Object, 
                _mockCookieStorageService.Object, 
                _mockCommitmentsApiClient.Object,
                _mockLinkGenerator.Object, 
                Mock.Of<ILogger<ApprenticeController>>(),
                Mock.Of<IAuthorizationService>());
        }

        [Test, MoqAutoData]
        public void AndPauseIsSelected_ThenRedirectToPauseApprenticeshipAction(ChangeStatusRequestViewModel viewModel)
        {
            viewModel.SelectedStatusChange = ChangeStatusType.Pause;
           
            var response = _controller.ChangeStatus(viewModel) as RedirectToActionResult;

            Assert.AreEqual("PauseApprenticeship", response.ActionName);
        }

        [Test, MoqAutoData]
        public void AndGoBackIsSelected_ThenRedirectToPauseApprenticeshipAction(ChangeStatusRequestViewModel viewModel)
        {
            viewModel.SelectedStatusChange = ChangeStatusType.GoBack;

            var response = _controller.ChangeStatus(viewModel) as RedirectToActionResult;

            Assert.AreEqual("ApprenticeshipDetails", response.ActionName);
        }

        [Test, MoqAutoData]
        public void AndStopIsSelected_ThenRedirectToV1WhenToApplyStopAction(ChangeStatusRequestViewModel viewModel)
        {
            viewModel.SelectedStatusChange = ChangeStatusType.Stop;
            viewModel.CurrentStatus = CommitmentsV2.Types.ApprenticeshipStatus.Live;
            var response = _controller.ChangeStatus(viewModel);
            response.VerifyReturnsRedirectToActionResult().WithActionName("StopApprenticeship");
        }

        [Test, MoqAutoData]
        public void AndStopIsSelected_ThenRedirectToV1WhenToHasTheApprenticeBeenMadeRedundant(ChangeStatusRequestViewModel viewModel)
        {
            viewModel.SelectedStatusChange = ChangeStatusType.Stop;
            viewModel.CurrentStatus = CommitmentsV2.Types.ApprenticeshipStatus.WaitingToStart;
            var response = _controller.ChangeStatus(viewModel);
            response.VerifyReturnsRedirectToActionResult().WithActionName("HasTheApprenticeBeenMadeRedundant");
        }
    }
}

using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.WhyStopApprenticeshipViewModel;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class ChangeStopApprenticeStatusChangeTests : ApprenticeControllerTestBase
    {

        [Test, MoqAutoData]
        public void AndLeftEmploymentIsSelected_ThenRedirectToStopApprenticeshipAction([NoAutoProperties] ApprenticeController controller, WhyStopApprenticeshipViewModel viewModel)
        {
            //Arrange
            viewModel.SelectedStatusChange = StopStatusReason.LeftEmployment;

            //Act
            var response = controller.WhyStopApprenticeship(viewModel) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("StopApprenticeship", response.ActionName);
        }

        [Test, MoqAutoData]
        public void AndChangeProviderIsSelected_ThenRedirectToStopApprenticeshipAction([NoAutoProperties] ApprenticeController controller, WhyStopApprenticeshipViewModel viewModel)
        {
            //Arrange
            viewModel.SelectedStatusChange = StopStatusReason.ChangeProvider;

            //Act
            var response = controller.WhyStopApprenticeship(viewModel) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("StopApprenticeship", response.ActionName);
        }

        [Test, MoqAutoData]
        public void AndWithdrawnIsSelected_ThenRedirectToStopApprenticeshipAction([NoAutoProperties] ApprenticeController controller, WhyStopApprenticeshipViewModel viewModel)
        {
            //Arrange
            viewModel.SelectedStatusChange = StopStatusReason.Withdrawn;

            //Act
            var response = controller.WhyStopApprenticeship(viewModel) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("StopApprenticeship", response.ActionName);
        }

        [Test, MoqAutoData]
        public void AndNeverStartedIsSelected_ThenRedirectToStopApprenticeshipAction([NoAutoProperties] ApprenticeController controller, WhyStopApprenticeshipViewModel viewModel)
        {
            //Arrange
            viewModel.SelectedStatusChange = StopStatusReason.NeverStarted;

            //Act
            var response = controller.WhyStopApprenticeship(viewModel) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("ApprenticeshipNeverStarted", response.ActionName);
        }

        [Test, MoqAutoData]
        public void AndProviderCorrectsApprenticeRecordIsSelected_ThenRedirectToStopApprenticeshipAction([NoAutoProperties] ApprenticeController controller, WhyStopApprenticeshipViewModel viewModel)
        {
            //Arrange
            viewModel.SelectedStatusChange = StopStatusReason.ProviderCorrectsApprenticeRecord;

            //Act
            var response = controller.WhyStopApprenticeship(viewModel) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("StopApprenticeship", response.ActionName);
        }

        [Test, MoqAutoData]
        public void AndNeverEndedIsSelected_ThenRedirectToTrainingEnded([NoAutoProperties] ApprenticeController controller, WhyStopApprenticeshipViewModel viewModel)
        {
            //Arrange
            viewModel.SelectedStatusChange = StopStatusReason.TrainingEnded;

            //Act
            var response = controller.WhyStopApprenticeship(viewModel) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("ApprenticeshipNotEnded", response.ActionName);
        }
    }
}

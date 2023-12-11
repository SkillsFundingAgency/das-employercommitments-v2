using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenPostingChangeStatus : ApprenticeControllerTestBase
{
    [SetUp]
    public void Arrange()
    {
        MockModelMapper = new Mock<IModelMapper>();
        MockCookieStorageService = new Mock<ICookieStorageService<IndexRequest>>();
        MockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

        Controller = new ApprenticeController(MockModelMapper.Object,
            MockCookieStorageService.Object,
            MockCommitmentsApiClient.Object,
            Mock.Of<ILogger<ApprenticeController>>());
    }

    [Test, MoqAutoData]
    public void AndPauseIsSelected_ThenRedirectToPauseApprenticeshipAction(ChangeStatusRequestViewModel viewModel)
    {
        //Arrange
        viewModel.SelectedStatusChange = ChangeStatusType.Pause;

        //Act
        var response = Controller.ChangeStatus(viewModel) as RedirectToActionResult;

        //Assert
        Assert.That(response.ActionName, Is.EqualTo("PauseApprenticeship"));           
    }

    [Test, MoqAutoData]
    public void AndGoBackIsSelected_ThenRedirectToPauseApprenticeshipAction(ChangeStatusRequestViewModel viewModel)
    {
        //Arrange
        viewModel.SelectedStatusChange = ChangeStatusType.GoBack;

        //Act
        var response = Controller.ChangeStatus(viewModel) as RedirectToActionResult;

        //Assert
        Assert.That(response.ActionName, Is.EqualTo("ApprenticeshipDetails"));
    }

    [Test, MoqAutoData]
    public void AndStopIsSelected_ThenRedirectToV1WhenToApplyStopAction(ChangeStatusRequestViewModel viewModel)
    {
        //Arrange
        viewModel.SelectedStatusChange = ChangeStatusType.Stop;
        viewModel.CurrentStatus = CommitmentsV2.Types.ApprenticeshipStatus.Live;

        //Act
        var response = Controller.ChangeStatus(viewModel) as RedirectToActionResult;

        //Assert
        Assert.That(response.ActionName, Is.EqualTo("WhyStopApprenticeship"));           
    }

    [Test, MoqAutoData]
    public void AndStopIsSelected_ThenRedirectToV1WhenToHasTheApprenticeBeenMadeRedundant(ChangeStatusRequestViewModel viewModel)
    {
        //Arrange
        viewModel.SelectedStatusChange = ChangeStatusType.Stop;
        viewModel.CurrentStatus = CommitmentsV2.Types.ApprenticeshipStatus.WaitingToStart;

        //Act
        var response = Controller.ChangeStatus(viewModel) as RedirectToActionResult;

        //Assert
        Assert.That(response.ActionName, Is.EqualTo("HasTheApprenticeBeenMadeRedundant"));
    }

    [Test, MoqAutoData]
    public void AndPauseIsSelected_ThenRedirectToResumeApprenticeshipAction(ChangeStatusRequestViewModel viewModel)
    {
        //Arrange
        viewModel.SelectedStatusChange = ChangeStatusType.Resume;

        //Act
        var response = Controller.ChangeStatus(viewModel) as RedirectToActionResult;

        //Assert
        Assert.That(response.ActionName, Is.EqualTo("ResumeApprenticeship"));         
    }
}
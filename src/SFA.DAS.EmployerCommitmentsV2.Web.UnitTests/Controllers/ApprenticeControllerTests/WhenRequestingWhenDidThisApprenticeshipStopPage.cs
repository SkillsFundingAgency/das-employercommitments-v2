using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenRequestingWhenDidThisApprenticeshipStopPage : ApprenticeControllerTestBase
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

    [Test]
    public async Task WhenRequesting_WhenDidThisApprenticeshipStop_ThenStopRequestViewModelIsPassedToTheView()
    {
        var requestViewModel = new StopRequestViewModel { StopMonth = 6, StopYear = 2020, ApprenticeshipId = 1, AccountHashedId = "AAXX" };
        MockModelMapper.Setup(m => m.Map<StopRequestViewModel>(It.IsAny<StopRequest>()))
            .ReturnsAsync(requestViewModel);

        var viewResult = await Controller.StopApprenticeship(new StopRequest()) as ViewResult;
        var viewModel = viewResult.Model;

        var stopRequestViewModel = (StopRequestViewModel)viewModel;

        Assert.Multiple(() =>
        {
            Assert.That(viewModel, Is.InstanceOf<StopRequestViewModel>());
            Assert.That(stopRequestViewModel, Is.EqualTo(requestViewModel));
        });
    }
}
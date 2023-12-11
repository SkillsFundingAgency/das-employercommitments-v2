using System.Threading.Tasks;
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

public class WhenSelectedApprenticeNeverEnded:ApprenticeControllerTestBase
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
    public async Task WhenSelected_ApprenticeshipNotEnded_ThenApprenticeshipNotEndedViewModelIsPassedToTheView(ApprenticeshipNotEndedViewModel expectedViewModel)
    {
        MockModelMapper.Setup(m => m.Map<ApprenticeshipNotEndedViewModel>(It.IsAny<ApprenticeshipNotEndedRequest>()))
            .ReturnsAsync(expectedViewModel);

        var viewResult = await Controller.ApprenticeshipNotEnded(new ApprenticeshipNotEndedRequest()) as ViewResult;
        var viewModel = viewResult.Model;

        var actualViewModel = (ApprenticeshipNotEndedViewModel)viewModel;

        Assert.That(viewModel, Is.InstanceOf<ApprenticeshipNotEndedViewModel>());
        Assert.That(actualViewModel, Is.EqualTo(expectedViewModel));
    }
}
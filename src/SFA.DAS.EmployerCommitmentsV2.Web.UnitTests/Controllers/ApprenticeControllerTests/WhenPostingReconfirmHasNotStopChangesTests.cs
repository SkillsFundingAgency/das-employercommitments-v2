﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

[TestFixture]
public class WhenPostingReconfirmHasNotStopChangesTests : ApprenticeControllerTestBase
{
    [SetUp]
    public void Arrange()
    {
        var fixture = new Fixture();
        MockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        MockModelMapper = new Mock<IModelMapper>();
        CacheStorageService = new Mock<Interfaces.ICacheStorageService>();

        Controller = new ApprenticeController(MockModelMapper.Object,
            Mock.Of<Interfaces.ICookieStorageService<IndexRequest>>(),
            MockCommitmentsApiClient.Object,
            CacheStorageService.Object,
            Mock.Of<ILogger<ApprenticeController>>());

        Controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
    }

    [Test, MoqAutoData]
    public async Task AndTheApprenticeship_IsNotStoppedIsConfirmed_ThenRedirectToApprenticeshipDetails(ReconfirmHasNotStopViewModel viewModel)
    {
        var result = await Controller.ReconfirmHasNotStopChangesAsync(viewModel);

        var redirect = result.VerifyReturnsRedirectToActionResult();

        Assert.Multiple(() =>
        {
            Assert.That(Controller.TempData.Values.Contains($"Apprenticeship confirmed"), Is.True);
            Assert.That(redirect.ActionName, Is.EqualTo("ApprenticeshipDetails"));
            Assert.That(viewModel.AccountHashedId, Is.EqualTo(redirect.RouteValues["AccountHashedId"]));
            Assert.That(viewModel.ApprenticeshipHashedId, Is.EqualTo(redirect.RouteValues["ApprenticeshipHashedId"]));
        });
    }

    [Test, MoqAutoData]
    public async Task AndTheApprenticeship_IsNotStoppedIsConfirmed_CommitmentApi_IsCalled(ReconfirmHasNotStopViewModel viewModel)
    {
        //Arrange
        viewModel.StopConfirmed = true;

        //Act
        await Controller.ReconfirmHasNotStopChangesAsync(viewModel);

        //Assert
        MockCommitmentsApiClient.Verify(x => x.ResolveOverlappingTrainingDateRequest(It.IsAny<ResolveApprenticeshipOverlappingTrainingDateRequest>(), CancellationToken.None), Times.Once);
    }
}
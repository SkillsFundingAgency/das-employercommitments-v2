using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenPostingEnterNewTrainingProvider
{
    private WhenPostingEnterNewTrainingProviderFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenPostingEnterNewTrainingProviderFixture();
    }

    [Test]
    public async Task ThenRedirectToWhoWillEnterTheDetailsPage()
    {
        var result = await _fixture.EnterNewTrainingProvider();

        WhenPostingEnterNewTrainingProviderFixture.VerifyRedirectsToWhoWillEnterTheDetailsPage(result);
    }
}

public class WhenPostingEnterNewTrainingProviderFixture : ApprenticeControllerTestFixtureBase
{
    private readonly EnterNewTrainingProviderViewModel _viewModel;

    public WhenPostingEnterNewTrainingProviderFixture()
    {
        _viewModel = AutoFixture.Create<EnterNewTrainingProviderViewModel>();

        MockMapper.Setup(m => m.Map<WhoWillEnterTheDetailsRequest>(_viewModel))
            .ReturnsAsync(new WhoWillEnterTheDetailsRequest { AccountHashedId = _viewModel.AccountHashedId, ApprenticeshipHashedId = _viewModel.ApprenticeshipHashedId, ProviderId = _viewModel.ProviderId.Value });
        MockMapper.Setup(m => m.Map<SendNewTrainingProviderRequest>(_viewModel))
            .ReturnsAsync(new SendNewTrainingProviderRequest { AccountHashedId = _viewModel.AccountHashedId, ApprenticeshipHashedId = _viewModel.ApprenticeshipHashedId, ProviderId = _viewModel.ProviderId.Value });
    }

    public async Task<IActionResult> EnterNewTrainingProvider()
    {
        return await Controller.EnterNewTrainingProvider(_viewModel);
    }

    public static void VerifyRedirectsToWhoWillEnterTheDetailsPage(IActionResult result)
    {
        var redirect = (RedirectToRouteResult)result;

        Assert.That(redirect.RouteName, Is.EqualTo(RouteNames.WhoWillEnterTheDetails));
    }
}
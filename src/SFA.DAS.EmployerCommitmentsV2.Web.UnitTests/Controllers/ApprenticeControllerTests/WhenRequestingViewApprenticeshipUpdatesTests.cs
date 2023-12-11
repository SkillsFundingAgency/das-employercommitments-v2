using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenRequestingViewApprenticeshipUpdatesTests
{
    private WhenRequestingViewApprenticeshipUpdatesTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenRequestingViewApprenticeshipUpdatesTestsFixture();
    }   

    [Test]
    public async Task VerifyViewModelMapperIsCalled()
    {
        await _fixture.ViewApprenticeshipUpdates();
        _fixture.VerifyViewModelMapperIsCalled();
    }

    [Test]
    public async Task VerifyViewIsReturned()
    {
        var result =  await _fixture.ViewApprenticeshipUpdates();
        WhenRequestingViewApprenticeshipUpdatesTestsFixture.VerifyViewResultIsReturned(result);
    }
}

public class WhenRequestingViewApprenticeshipUpdatesTestsFixture : ApprenticeControllerTestFixtureBase
{
    public ViewApprenticeshipUpdatesRequest Request { get; set; }
    public WhenRequestingViewApprenticeshipUpdatesTestsFixture()
    {
        Request = new ViewApprenticeshipUpdatesRequest { ApprenticeshipId = 1, AccountId = 1 };
        Controller.TempData = new TempDataDictionary( Mock.Of<HttpContext>(), Mock.Of<ITempDataProvider>());
    }

    public async Task<IActionResult> ViewApprenticeshipUpdates()
    {
        return await Controller.ViewApprenticeshipUpdates(Request);
    }     

    internal void VerifyViewModelMapperIsCalled()
    {
        MockMapper.Verify(x => x.Map<ViewApprenticeshipUpdatesViewModel>(It.IsAny<ViewApprenticeshipUpdatesRequest>()), Times.Once());
    }

    internal static void VerifyViewResultIsReturned(IActionResult result)
    {
        Assert.That(result, Is.InstanceOf<ViewResult>());
    }
}
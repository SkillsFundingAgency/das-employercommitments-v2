using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenRequestingReviewApprenticeshipUpdatesTests
{
    private WhenRequestingReviewApprenticeshipUpdatesTestsixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenRequestingReviewApprenticeshipUpdatesTestsixture();
    }   

    [Test]
    public async Task VerifyViewModelMapperIsCalled()
    {
        await _fixture.ReviewApprenticeshipUpdates();
        _fixture.VerifyViewModelMapperIsCalled();
    }

    [Test]
    public async Task VerifyViewIsReturned()
    {
        var result =  await _fixture.ReviewApprenticeshipUpdates();
        WhenRequestingReviewApprenticeshipUpdatesTestsixture.VerifyViewResultIsReturned(result);
    }
}

public class WhenRequestingReviewApprenticeshipUpdatesTestsixture : ApprenticeControllerTestFixtureBase
{
    public ReviewApprenticeshipUpdatesRequest Request { get; set; }
    public WhenRequestingReviewApprenticeshipUpdatesTestsixture() : base () 
    {
        Request = new ReviewApprenticeshipUpdatesRequest { ApprenticeshipId = 1, AccountId = 1 };
        Controller.TempData = new TempDataDictionary( Mock.Of<HttpContext>(), Mock.Of<ITempDataProvider>());
    }

    public async Task<IActionResult> ReviewApprenticeshipUpdates()
    {
        return await Controller.ReviewApprenticeshipUpdates(Request);
    }     

    internal void VerifyViewModelMapperIsCalled()
    {
        MockMapper.Verify(x => x.Map<ReviewApprenticeshipUpdatesViewModel>(It.IsAny<ReviewApprenticeshipUpdatesRequest>()), Times.Once());
    }

    internal static void VerifyViewResultIsReturned(IActionResult result)
    {
        Assert.That(result, Is.InstanceOf<ViewResult>());
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenRequestingConfirmEditApprenticeshipTests
{
    private WhenRequestingConfirmEditApprenticeshipFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenRequestingConfirmEditApprenticeshipFixture();
    }   

    [Test]
    public async Task VerifyViewModelMapperIsCalled()
    {
        await _fixture.ConfirmEditApprenticeship();
        _fixture.VerifyViewModelMapperIsCalled();
    }

    [Test]
    public async Task VerifyViewIsReturned()
    {
        var result =  await _fixture.ConfirmEditApprenticeship();
        WhenRequestingConfirmEditApprenticeshipFixture.VerifyViewResultIsReturned(result);
    }
}

public class WhenRequestingConfirmEditApprenticeshipFixture : ApprenticeControllerTestFixtureBase
{
    public WhenRequestingConfirmEditApprenticeshipFixture() : base () 
    {
        Controller.TempData = new TempDataDictionary( Mock.Of<HttpContext>(), Mock.Of<ITempDataProvider>());
    }

    public async Task<IActionResult> ConfirmEditApprenticeship()
    {
        return await Controller.ConfirmEditApprenticeship();
    }     
    
    internal void VerifyViewModelMapperIsCalled()
    {
        MockMapper.Verify(x => x.Map<ConfirmEditApprenticeshipViewModel>(It.IsAny<EditApprenticeshipRequestViewModel>()), Times.Once());
    }

    internal static void VerifyViewResultIsReturned(IActionResult result)
    {
        Assert.That(result, Is.InstanceOf<ViewResult>());
    }
}
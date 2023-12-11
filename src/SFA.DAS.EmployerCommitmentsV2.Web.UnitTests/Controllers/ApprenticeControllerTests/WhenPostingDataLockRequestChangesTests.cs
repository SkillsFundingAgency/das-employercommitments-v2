using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenPostingDataLockRequestChangesTests
{
    private WhenPostingDataLockRequestChangesTestsFixture _fixture;
    private DataLockRequestChangesViewModel _viewModel;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenPostingDataLockRequestChangesTestsFixture();
        _viewModel = new DataLockRequestChangesViewModel();
    }   

    [Test]
    public async Task VerifyRejectIsCalled_WhenNotApprovingRequest()
    {
        _viewModel.AcceptChanges = false;
        await _fixture.DataLockRequestChanges(_viewModel);
        _fixture.VerifyRejectIsCalledOnApi();
    }

    [Test]
    public async Task VerifyAcceptIsCalled_WhenApprovingRequest()
    {
        _viewModel.AcceptChanges = true;
        await _fixture.DataLockRequestChanges(_viewModel);
        _fixture.VerifyAcceptIsCalledOnApi();
    }
}

public class WhenPostingDataLockRequestChangesTestsFixture : ApprenticeControllerTestFixtureBase
{
    public WhenPostingDataLockRequestChangesTestsFixture() : base () 
    {
        Controller.TempData = new TempDataDictionary( Mock.Of<HttpContext>(), Mock.Of<ITempDataProvider>());
    }

    public async Task DataLockRequestChanges(DataLockRequestChangesViewModel viewModel)
    {
        await Controller.DataLockRequestChanges(viewModel);
    }

    public void VerifyRejectIsCalledOnApi()
    {
        MockCommitmentsApiClient.Verify(x => x.RejectDataLockChanges(It.IsAny<long>(), It.IsAny<RejectDataLocksRequestChangesRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    public void VerifyAcceptIsCalledOnApi()
    {
        MockCommitmentsApiClient.Verify(x => x.AcceptDataLockChanges(It.IsAny<long>(), It.IsAny<AcceptDataLocksRequestChangesRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenCallingDataLockRequestRestartTests
{
    private WhenCallingDataLockRequestRestartTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenCallingDataLockRequestRestartTestsFixture();
    }

    public async Task ThenTheCorrectViewIsReturned()
    {
        var result = await _fixture.DataLockRequestRestart();

        _fixture.VerifyViewModel(result as ViewResult);
    }
}

public class WhenCallingDataLockRequestRestartTestsFixture : ApprenticeControllerTestFixtureBase
{
    private readonly DataLockRequestRestartRequest _request;
    private readonly DataLockRequestRestartViewModel _viewModel;

    public WhenCallingDataLockRequestRestartTestsFixture() : base()
    {
        _request = AutoFixture.Create<DataLockRequestRestartRequest>();
        _viewModel = AutoFixture.Create<DataLockRequestRestartViewModel>();

        MockMapper.Setup(m => m.Map<DataLockRequestRestartViewModel>(_request))
            .ReturnsAsync(_viewModel);
    }

    public async Task<IActionResult> DataLockRequestRestart()
    {
        return await Controller.DataLockRequestRestart(_request);
    }

    public void VerifyViewModel(ViewResult viewResult)
    {
        var viewModel = viewResult.Model as DataLockRequestRestartViewModel;

        Assert.That(viewModel, Is.InstanceOf<DataLockRequestRestartViewModel>());
        Assert.That(viewModel, Is.EqualTo(_viewModel));
    }
}
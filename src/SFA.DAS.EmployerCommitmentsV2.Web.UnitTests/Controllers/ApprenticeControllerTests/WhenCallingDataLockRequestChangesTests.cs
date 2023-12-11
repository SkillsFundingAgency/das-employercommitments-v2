using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenCallingDataLockRequestChangesTests
{
    private WhenCallingDataLockRequestChangesTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenCallingDataLockRequestChangesTestsFixture();
    }

    [Test]
    public async Task ThenTheCorrectViewIsReturned()
    {
        var result = await _fixture.DataLockRequestChanges();

        _fixture.VerifyViewModel(result as ViewResult);
    }
}

public class WhenCallingDataLockRequestChangesTestsFixture : ApprenticeControllerTestFixtureBase
{
    private readonly DataLockRequestChangesRequest _request;
    private readonly DataLockRequestChangesViewModel _viewModel;

    public WhenCallingDataLockRequestChangesTestsFixture()
    {
        _request = AutoFixture.Create<DataLockRequestChangesRequest>();
        _viewModel = AutoFixture.Create<DataLockRequestChangesViewModel>();
            

        MockMapper.Setup(m => m.Map<DataLockRequestChangesViewModel>(_request))
            .ReturnsAsync(_viewModel);
    }

    public async Task<IActionResult> DataLockRequestChanges()
    {
        return await Controller.DataLockRequestChanges(_request);
    }

    public void VerifyViewModel(ViewResult viewResult)
    {
        var viewModel = viewResult.Model as DataLockRequestChangesViewModel;

        Assert.That(viewModel, Is.InstanceOf<DataLockRequestChangesViewModel>());
        Assert.That(viewModel, Is.EqualTo(_viewModel));
    }
}
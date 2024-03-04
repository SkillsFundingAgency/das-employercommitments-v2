using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenCallingEditApprenticeshipTests
{
    private WhenCallingEditApprenticeshipTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenCallingEditApprenticeshipTestsFixture();
    }

    [Test]
    public async Task ThenTheCorrectViewIsReturned()
    {
        var result = await _fixture.EditApprenticeship();

        _fixture.VerifyViewModel(result as ViewResult);
    }

    [Test]
    public async Task AndWeHaveAnExistingEditViewModel_ThenTheTempModelIsPassedToTheView()
    {
        var result = await _fixture.WithTempModel().EditApprenticeship();
        _fixture.VerifyViewModelIsEquivalentToTempViewModel(result as ViewResult);
    }
}

public class WhenCallingEditApprenticeshipTestsFixture : ApprenticeControllerTestFixtureBase
{
    private readonly EditApprenticeshipRequest _request;
    private readonly EditApprenticeshipRequestViewModel _viewModel;
    private readonly EditApprenticeshipRequestViewModel _tempViewModel;
    private object _tempViewModelAsString;


    public WhenCallingEditApprenticeshipTestsFixture() : base()
    {
        _request = AutoFixture.Create<EditApprenticeshipRequest>();
        _viewModel = AutoFixture.Build<EditApprenticeshipRequestViewModel>()
            .Without(x => x.StartDate).Without(x => x.StartMonth).Without(x => x.StartYear)
            .Without(x => x.EndDate).Without(x => x.EndMonth).Without(x => x.EndYear)
            .Without(x => x.EmploymentEndDate).Without(x => x.EmploymentEndMonth).Without(x => x.EmploymentEndYear)
            .Create();

        _tempViewModel = AutoFixture.Build<EditApprenticeshipRequestViewModel>()
            .Without(x => x.StartDate).Without(x => x.StartMonth).Without(x => x.StartYear)
            .Without(x => x.EndDate).Without(x => x.EndMonth).Without(x => x.EndYear)
            .Without(x => x.EmploymentEndDate).Without(x => x.EmploymentEndMonth).Without(x => x.EmploymentEndYear)
            .Create(); ;
        _tempViewModelAsString = JsonConvert.SerializeObject(_tempViewModel);

        MockMapper.Setup(m => m.Map<EditApprenticeshipRequestViewModel>(_request))
            .ReturnsAsync(_viewModel);
    }

    public async Task<IActionResult> EditApprenticeship()
    {
        return await Controller.EditApprenticeship(_request);
    }

    public WhenCallingEditApprenticeshipTestsFixture WithTempModel()
    {
        TempDataDictionary.Setup(x => x.TryGetValue("ViewModelForEdit", out _tempViewModelAsString));
        _viewModel.DeliveryModel = DeliveryModel.PortableFlexiJob;
        return this;
    }

    public void VerifyViewModel(ViewResult viewResult)
    {
        var viewModel = viewResult.Model as EditApprenticeshipRequestViewModel;

        Assert.That(viewModel, Is.InstanceOf<EditApprenticeshipRequestViewModel>());
        Assert.That(viewModel, Is.EqualTo(_viewModel));
    }

    public void VerifyViewModelIsEquivalentToTempViewModel(ViewResult viewResult)
    {
        var viewModel = viewResult.Model as EditApprenticeshipRequestViewModel;

        Assert.That(viewModel, Is.InstanceOf<EditApprenticeshipRequestViewModel>());
        _tempViewModel.Should().BeEquivalentTo(viewModel);
    }
}
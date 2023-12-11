using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenCallingEditStopDateTests
{
    private WhenCallingEditStopDateTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenCallingEditStopDateTestsFixture();
    }

    [Test]
    public async Task ThenTheCorrectViewIsReturned()
    {
        //Act
        var result = await _fixture.EditStopDate();

        //Assert
        _fixture.VerifyViewModel(result as ViewResult);
    }
}

public class WhenCallingEditStopDateTestsFixture : ApprenticeControllerTestFixtureBase
{
    private readonly EditStopDateRequest _request;
    private readonly EditStopDateViewModel _viewModel;

    public WhenCallingEditStopDateTestsFixture()
    {
        _request = AutoFixture.Create<EditStopDateRequest>();           
        AutoFixture.Customize<EditStopDateViewModel>(c => c.Without(x => x.NewStopDate));
        _viewModel = AutoFixture.Create<EditStopDateViewModel>();
        _viewModel.NewStopDate = new CommitmentsV2.Shared.Models.MonthYearModel("062020");           

        MockMapper.Setup(m => m.Map<EditStopDateViewModel>(_request))
            .ReturnsAsync(_viewModel);
    }

    public async Task<IActionResult> EditStopDate()
    {
        return await Controller.EditStopDate(_request);
    }

    public void VerifyViewModel(ViewResult viewResult)
    {
        var viewModel = viewResult.Model as EditStopDateViewModel;

        Assert.That(viewModel, Is.InstanceOf<EditStopDateViewModel>());
        Assert.That(viewModel, Is.EqualTo(_viewModel));
    }
}
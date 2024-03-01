using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.DraftApprenticeshipControllerTests;

[TestFixture]
public class SelectOptionTests
{
    private Mock<IModelMapper> _mockModelMapper;
    private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
    private Mock<IApprovalsApiClient> _mockOuterApiClient;

    private SelectOptionRequest _request;
    private SelectOptionViewModel _viewModel;
    private UpdateDraftApprenticeshipApimRequest _updateRequest;

    private DraftApprenticeshipController _controller;

    [SetUp]
    public void Arrange()
    {
        var fixture = new Fixture();

        _request = fixture.Create<SelectOptionRequest>();

        var birthDate = fixture.Create<DateTime?>();
        var startDate = fixture.Create<DateTime?>();
        var endDate = fixture.Create<DateTime?>();

        _viewModel = fixture.Build<SelectOptionViewModel>()
            .With(x => x.BirthDay, birthDate?.Day)
            .With(x => x.BirthMonth, birthDate?.Month)
            .With(x => x.BirthYear, birthDate?.Year)
            .With(x => x.EndMonth, endDate?.Month)
            .With(x => x.EndYear, endDate?.Year)
            .With(x => x.StartMonth, startDate?.Month)
            .With(x => x.StartYear, startDate?.Year)
            .Without(x => x.StartDate)
            .Create();

        _updateRequest = fixture.Create<UpdateDraftApprenticeshipApimRequest>();

        _mockModelMapper = new Mock<IModelMapper>();
        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _mockOuterApiClient = new Mock<IApprovalsApiClient>();

        _controller = new DraftApprenticeshipController(_mockModelMapper.Object, _mockCommitmentsApiClient.Object, Mock.Of<IEncodingService>(), _mockOuterApiClient.Object);

        _mockModelMapper.Setup(m => m.Map<UpdateDraftApprenticeshipApimRequest>(_viewModel))
            .ReturnsAsync(_updateRequest);
    }
        
    [TearDown]
    public void TearDown() => _controller?.Dispose();

    [Test]
    public async Task WhenGettingSelectOption_And_StandardHasOptions_Then_ReturnViewModel()
    {
        _mockModelMapper.Setup(m => m.Map<SelectOptionViewModel>(_request))
            .ReturnsAsync(_viewModel);

        var result = await _controller.SelectOption(_request) as ViewResult;

        result.ViewName.Should().BeNull();

        var model = result.Model as SelectOptionViewModel;

        model.Should().BeEquivalentTo(_viewModel);
    }

    [Test]
    public async Task When_GettingSelectOption_And_MapperReturnsNullAsStandardVersionHasNoOptions_Then_RedirectToCohortDetails()
    {
        _mockModelMapper.Setup(m => m.Map<SelectOptionViewModel>(_request))
            .ReturnsAsync((SelectOptionViewModel)null);

        var result = await _controller.SelectOption(_request) as RedirectToActionResult;

        result.ActionName.Should().Be("Details");
    }

    [Test]
    public async Task When_PostingSelectOption_Then_CallUpdateDraftApprenticeshipApi()
    {
        var result = await _controller.SelectOption(_viewModel);

        _mockOuterApiClient.Verify(c => 
                c.UpdateDraftApprenticeship(_viewModel.CohortId.Value, _viewModel.DraftApprenticeshipId, _updateRequest, It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    public async Task When_PostingSelectOption_Then_RedirectToCohortDetailsPage()
    {
        var result = await _controller.SelectOption(_viewModel) as RedirectToActionResult;

        result.ControllerName.Should().Be("Cohort");
        result.ActionName.Should().Be("Details");
    }
}
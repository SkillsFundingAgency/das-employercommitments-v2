using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class GetAllChangeHistoryRequestToViewModelMapperTests
{
    private Fixture _fixture;
    private GetAllChangeHistoryRequest _request;
    private GetAllChangeHistoryResponse _response;
    private Mock<IApprovalsApiClient> _mockApprovalsApiClient;
    private GetAllChangeHistoryRequestToViewModelMapper _mapper;

    [SetUp]
    public void Arrange()
    {
        _fixture = new Fixture();

        _request = _fixture.Build<GetAllChangeHistoryRequest>()
            .With(x => x.PageNumber, 1)
            .Create();

        _response = _fixture.Build<GetAllChangeHistoryResponse>()
            .With(x => x.ChangeHistory, _fixture.CreateMany<GetChangeHistoryItem>(120).ToList())
            .Create();

        _mockApprovalsApiClient = new Mock<IApprovalsApiClient>();
        _mockApprovalsApiClient
            .Setup(c => c.GetChangeHistoryForEmployer(_request.AccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_response);

        _mapper = new GetAllChangeHistoryRequestToViewModelMapper(_mockApprovalsApiClient.Object);
    }

    [Test]
    public async Task Then_PageSizeIsFiftyChanges()
    {
        var viewModel = await _mapper.Map(_request);

        viewModel.PageSize.Should().Be(50);
        viewModel.ChangeHistory.Should().HaveCount(50);
    }

    [Test]
    public async Task Then_FewerThanFiftyChangesReturnsAllOnFirstPage()
    {
        _response.ChangeHistory = _fixture.CreateMany<GetChangeHistoryItem>(30).ToList();

        var viewModel = await _mapper.Map(_request);

        viewModel.ChangeHistory.Should().HaveCount(30);
        viewModel.TotalCount.Should().Be(30);
        viewModel.PageNumber.Should().Be(1);
        viewModel.PageSize.Should().Be(50);
    }

    [Test]
    public async Task Then_TotalCountIsAllChanges()
    {
        var viewModel = await _mapper.Map(_request);

        viewModel.TotalCount.Should().Be(120);
    }

    [Test]
    public async Task Then_SecondPageReturnsNextSlice()
    {
        _request.PageNumber = 2;

        var viewModel = await _mapper.Map(_request);

        viewModel.PageNumber.Should().Be(2);
        viewModel.ChangeHistory.Should().HaveCount(50);
        viewModel.ChangeHistory.First().Id.Should().Be(_response.ChangeHistory[50].Id);
    }

    [Test]
    public async Task Then_LastPageReturnsRemainingChanges()
    {
        _request.PageNumber = 3;

        var viewModel = await _mapper.Map(_request);

        viewModel.ChangeHistory.Should().HaveCount(20);
    }

    [Test]
    public async Task Then_PageNumberLessThanOneIsTreatedAsFirstPage()
    {
        _request.PageNumber = 0;

        var viewModel = await _mapper.Map(_request);

        viewModel.PageNumber.Should().Be(1);
        viewModel.ChangeHistory.First().Id.Should().Be(_response.ChangeHistory[0].Id);
    }

    [Test]
    public async Task Then_NullChangeHistoryReturnsEmptyViewModel()
    {
        _response.ChangeHistory = null;

        var viewModel = await _mapper.Map(_request);

        viewModel.ChangeHistory.Should().BeEmpty();
        viewModel.TotalCount.Should().Be(0);
    }

    [Test]
    public async Task Then_NullResponseReturnsEmptyViewModel()
    {
        _mockApprovalsApiClient
            .Setup(c => c.GetChangeHistoryForEmployer(_request.AccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetAllChangeHistoryResponse)null);

        var viewModel = await _mapper.Map(_request);

        viewModel.ChangeHistory.Should().BeEmpty();
        viewModel.TotalCount.Should().Be(0);
    }

    [Test]
    public async Task Then_EmptyChangeHistoryReturnsEmptyViewModelAndCurrentTimeForAvailableFrom()
    {
        _response.ChangeHistory = new List<GetChangeHistoryItem>();
        var before = DateTime.UtcNow;

        var viewModel = await _mapper.Map(_request);

        viewModel.ChangeHistory.Should().BeEmpty();
        viewModel.TotalCount.Should().Be(0);
        viewModel.AvailableFrom.Should().BeOnOrAfter(before);
    }

    [Test]
    public async Task Then_PageBeyondLastPageReturnsNoItems()
    {
        _request.PageNumber = 5;

        var viewModel = await _mapper.Map(_request);

        viewModel.ChangeHistory.Should().BeEmpty();
        viewModel.TotalCount.Should().Be(120);
    }

    [Test]
    public async Task Then_ExactMultipleOfPageSizeFillsLastPageWithoutEmptyPage()
    {
        _response.ChangeHistory = _fixture.CreateMany<GetChangeHistoryItem>(100).ToList();
        _request.PageNumber = 2;

        var viewModel = await _mapper.Map(_request);

        viewModel.ChangeHistory.Should().HaveCount(50);

        _request.PageNumber = 3;
        var emptyPage = await _mapper.Map(_request);

        emptyPage.ChangeHistory.Should().BeEmpty();
    }

    [Test]
    public async Task Then_AvailableFromIsEarliestCreatedDate()
    {
        var earliest = _response.ChangeHistory.Min(x => x.Created);

        var viewModel = await _mapper.Map(_request);

        viewModel.AvailableFrom.Should().Be(earliest);
    }
}

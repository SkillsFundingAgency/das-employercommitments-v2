using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ChangeHistoryRequestToViewModelMapperTests
{
    private Fixture _fixture;

    private ChangeHistoryRequest _request;

    private GetChangeHistoryResponse _getChangeHistoryResponse;
    private ChangeHistoryViewModel _historyViewModel;

    private Mock<IApprovalsApiClient> _mockApprovalsApiClient;
    private Mock<IEncodingService> _encodingService;
    private ChangeHistoryRequestToViewModelMapper _mapper;

    [SetUp]
    public void Arrange()
    {
        _fixture = new Fixture();

        _request = _fixture.Create<ChangeHistoryRequest>();

        var apprenticeShipId = 12345;

        _getChangeHistoryResponse = _fixture.Build<GetChangeHistoryResponse>()
            .With(x => x.ChangeHistory)
            .Create();

        _historyViewModel = _fixture.Create<ChangeHistoryViewModel>();

        _getChangeHistoryResponse = _fixture.Create<GetChangeHistoryResponse>();

        _mockApprovalsApiClient = new Mock<IApprovalsApiClient>();
        _mockApprovalsApiClient
            .Setup(c => c.GetChangeHistory(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getChangeHistoryResponse);
        _encodingService = new Mock<IEncodingService>();

        _encodingService
            .Setup(c => c.Decode(It.Is<string>(t => t == _request.ApprenticeshipHashedId), It.Is<EncodingType>(e => e == EncodingType.ApprenticeshipId)))
            .Returns(apprenticeShipId);

        _mapper = new ChangeHistoryRequestToViewModelMapper(_mockApprovalsApiClient.Object,
           _encodingService.Object);
    }

    [Test]
    public async Task Then_LearnerNameIsMapped()
    {
        var viewModel = await _mapper.Map(_request);

        viewModel.Name.Should().Be(_getChangeHistoryResponse.ChangeHistory.FirstOrDefault().LearnerName);
    }

    [Test]
    public async Task Then_ViewModelIsMapped()
    {
        var viewModel = await _mapper.Map(_request);

        foreach (var item in viewModel.ChangeHistory)
        {
            item.Description.Should().Be(_getChangeHistoryResponse.ChangeHistory.First(t => t.Id == item.Id).Description);
            item.ChangeType.Should().Be((LearningChangeType)_getChangeHistoryResponse.ChangeHistory.First(t => t.Id == item.Id).ChangeType);
            item.AppliedDate.Should().Be(_getChangeHistoryResponse.ChangeHistory.First(t => t.Id == item.Id).AppliedDate);
        }
    }
}
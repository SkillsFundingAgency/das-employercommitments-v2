using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class WhatIsTheNewStartDateViewModelMapperTests
{
    private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

    private ChangeOfProviderRequest _request;
    private GetApprenticeshipResponse _apprenticeshipResponse;

    private WhatIsTheNewStartDateViewModelMapper _mapper;

    [SetUp]
    public void Arrange()
    {
        var _autoFixture = new Fixture();

        _request = _autoFixture.Build<ChangeOfProviderRequest>()
            .With(x => x.NewStartMonth, 1)
            .With(x => x.NewStartYear, 2020)
            .With(x => x.NewEndMonth, 1)
            .With(x => x.NewEndYear, 2022)
            .Create();
        _apprenticeshipResponse = _autoFixture.Create<GetApprenticeshipResponse>();

        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _mockCommitmentsApiClient.Setup(m => m.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_apprenticeshipResponse);

        _mapper = new WhatIsTheNewStartDateViewModelMapper(_mockCommitmentsApiClient.Object);
    }

    [Test]
    public async Task ApprenticeshipHashedId_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(_request.ApprenticeshipHashedId));
    }

    [Test]
    public async Task AccountHashedId_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.AccountHashedId, Is.EqualTo(_request.AccountHashedId));
    }

    [Test]
    public async Task ProviderId_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.ProviderId, Is.EqualTo(_request.ProviderId));
    }

    [Test]
    public async Task ProviderName_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.ProviderName, Is.EqualTo(_request.ProviderName));
    }

    [Test]
    public async Task WhenRequestingTheWhatIsTheNewStartDatePage_ThenTheGetApprenticeshipIsCalled()
    {
        var result = await _mapper.Map(_request);

        _mockCommitmentsApiClient.Verify(m => m.GetApprenticeship(_request.ApprenticeshipId.Value, It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task StopDate_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.StopDate, Is.EqualTo(_apprenticeshipResponse.StopDate));
    }

    [TestCase(12)]
    [TestCase(null)]
    public async Task NewStartMonth_IsMapped(int? newStartMonth)
    {
        _request.NewEndMonth = newStartMonth;

        var result = await _mapper.Map(_request);

        Assert.That(result.NewStartMonth, Is.EqualTo(_request.NewStartMonth));
    }

    [TestCase(2020)]
    [TestCase(null)]
    public async Task NewStartYear_IsMapped(int? newStartYear)
    {
        _request.NewEndYear = newStartYear;

        var result = await _mapper.Map(_request);

        Assert.That(result.NewStartYear, Is.EqualTo(_request.NewStartYear));
    }

    [TestCase(12)]
    [TestCase(null)]
    public async Task NewEndMonth_IsMapped(int? newEndMonth)
    {
        _request.NewEndMonth = newEndMonth;

        var result = await _mapper.Map(_request);

        Assert.That(result.NewEndMonth, Is.EqualTo(_request.NewEndMonth));
    }

    [TestCase(2020)]
    [TestCase(null)]
    public async Task NewEndYear_IsMapped(int? newEndYear)
    {
        _request.NewEndYear = newEndYear;

        var result = await _mapper.Map(_request);

        Assert.That(result.NewEndYear, Is.EqualTo(_request.NewEndYear));
    }

    [TestCase(500)]
    [TestCase(null)]
    public async Task NewPrice_IsMapped(int? newPrice)
    {
        _request.NewPrice = newPrice;

        var result = await _mapper.Map(_request);

        Assert.That(result.NewPrice, Is.EqualTo(_request.NewPrice));
    }

    [TestCase(true, true)]
    [TestCase(null, false)]
    public async Task EditFlag_IsMapped(bool? edit, bool expectedResult)
    {
        _request.Edit = edit;

        var result = await _mapper.Map(_request);

        Assert.That(result.Edit, Is.EqualTo(expectedResult));
    }
}
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class WhatIsTheNewPriceViewModelMapperTests
{
    private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

    private ChangeOfProviderRequest _request;
    private GetApprenticeshipResponse _apprenticeshipResponse;

    private WhatIsTheNewPriceViewModelMapper _mapper;

    [SetUp]
    public void Arrange()
    {
        var _autoFixture = new Fixture();

        var test = DateTime.UtcNow.Year;

        _request = _autoFixture.Create<ChangeOfProviderRequest>();
        _request.NewStartYear = DateTime.UtcNow.Year;
        _request.NewStartMonth = DateTime.UtcNow.Month;
        _request.NewEndYear = DateTime.UtcNow.AddYears(1).Year;
        _request.NewEndMonth = DateTime.UtcNow.AddYears(1).Month;
        _apprenticeshipResponse = _autoFixture.Create<GetApprenticeshipResponse>();

        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _mockCommitmentsApiClient.Setup(m => m.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_apprenticeshipResponse);

        _mapper = new WhatIsTheNewPriceViewModelMapper(_mockCommitmentsApiClient.Object);
    }

    [Test]
    public async Task ApprenticeshipHashedId_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(_request.ApprenticeshipHashedId));
    }

    [Test]
    public async Task AccountHashedId_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.AccountHashedId, Is.EqualTo(_request.AccountHashedId));
    }

    [Test]
    public async Task ProviderId_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.ProviderId, Is.EqualTo(_request.ProviderId));
    }
    [Test]
    public async Task WhenRequestingTheWhatIsTheNewStartDatePage_ThenTheGetApprenticeshipIsCalled()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        _mockCommitmentsApiClient.Verify(m => m.GetApprenticeship(_request.ApprenticeshipId.Value, It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task StopDate_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.StopDate, Is.EqualTo(_apprenticeshipResponse.StopDate));
    }

    [Test]
    public async Task ProviderName_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.ProviderName, Is.EqualTo(_request.ProviderName));
    }

    [Test]
    public async Task NewPrice_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.NewPrice, Is.EqualTo(_request.NewPrice));
    }

    [TestCase(true, true)]
    [TestCase(null, false)]
    public async Task EditFlag_IsMapped(bool? edit, bool expectedResult)
    {
        //Arrange
        _request.Edit = edit;

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.Edit, Is.EqualTo(expectedResult));
    }
}
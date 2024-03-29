﻿using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class EditStopDateRequestToViewModelMapperTests
{
    private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;        
    private Mock<ILogger<EditStopDateRequestToViewModelMapper>> _logger;
    private EditStopDateRequest _request;
    private GetApprenticeshipResponse _apprenticeshipResponse;
    private EditStopDateRequestToViewModelMapper _mapper;

    [SetUp]
    public void Arrange()
    {
        var _autoFixture = new Fixture();
        _request = _autoFixture.Create<EditStopDateRequest>();

        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _logger = new Mock<ILogger<EditStopDateRequestToViewModelMapper>>();
        _apprenticeshipResponse = _autoFixture.Create<GetApprenticeshipResponse>();
        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _mockCommitmentsApiClient.Setup(m => m.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_apprenticeshipResponse);
        

        _mapper = new EditStopDateRequestToViewModelMapper(_mockCommitmentsApiClient.Object, _logger.Object);
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
    public async Task ApprenticeshipId_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.ApprenticeshipId, Is.EqualTo(_request.ApprenticeshipId));
    }

    [Test]
    public async Task WhenRequestingEditStopDate_ThenTheGetApprenticeshipIsCalled()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        _mockCommitmentsApiClient.Verify(m => m.GetApprenticeship(_request.ApprenticeshipId, It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task ApprenticeshipULN_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.ApprenticeshipULN, Is.EqualTo(_apprenticeshipResponse.Uln));
    }

    [Test]
    public async Task ApprenticeshipStartDate_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.ApprenticeshipStartDate, Is.EqualTo(_apprenticeshipResponse.StartDate));
    }

    [Test]
    public async Task CurrentStopDate_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.CurrentStopDate, Is.EqualTo(_apprenticeshipResponse.StopDate.Value));
    }
}
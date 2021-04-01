using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
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
            Assert.AreEqual(_request.ApprenticeshipHashedId, result.ApprenticeshipHashedId);
        }

        [Test]
        public async Task AccountHashedId_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_request.AccountHashedId, result.AccountHashedId);
        }

        [Test]
        public async Task ApprenticeshipId_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_request.ApprenticeshipId, result.ApprenticeshipId);
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
            Assert.AreEqual(_apprenticeshipResponse.Uln, result.ApprenticeshipULN);
        }

        [Test]
        public async Task ApprenticeshipStartDate_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.StartDate, result.ApprenticeshipStartDate);
        }

        [Test]
        public async Task CurrentStopDate_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.StopDate.Value, result.CurrentStopDate);
        }
    }
}

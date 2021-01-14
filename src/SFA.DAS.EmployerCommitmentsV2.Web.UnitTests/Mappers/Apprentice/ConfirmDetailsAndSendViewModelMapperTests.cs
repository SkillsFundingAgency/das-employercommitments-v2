using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ConfirmDetailsAndSendViewModelMapperTests
    {
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
        private Mock<ITrainingProgrammeApiClient> _mockTrainingProgrammeApiClient;

        private ChangeOfProviderRequest _request;

        private GetApprenticeshipResponse _apprenticeshipResponse;
        private StandardSummary _standardSummary;

        private ConfirmDetailsAndSendViewModelMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();

            _request = autoFixture.Create<ChangeOfProviderRequest>();

            _apprenticeshipResponse = autoFixture.Build<GetApprenticeshipResponse>()
                .With(x => x.CourseCode, "ABC").Create();

            _standardSummary = autoFixture.Build<StandardSummary>()
                .With(x => x.CurrentFundingCap, 1000).Create();

            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockCommitmentsApiClient.Setup(c => c.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_apprenticeshipResponse);

            _mockTrainingProgrammeApiClient = new Mock<ITrainingProgrammeApiClient>();
            _mockTrainingProgrammeApiClient.Setup(t => t.GetTrainingProgramme(_apprenticeshipResponse.CourseCode))
                .ReturnsAsync(_standardSummary);

            _mapper = new ConfirmDetailsAndSendViewModelMapper(_mockCommitmentsApiClient.Object, _mockTrainingProgrammeApiClient.Object);
        }

        [Test]
        public async Task GetFundingCapIsCalled()
        {
            var result = await _mapper.Map(_request);
        }

    

    }
}

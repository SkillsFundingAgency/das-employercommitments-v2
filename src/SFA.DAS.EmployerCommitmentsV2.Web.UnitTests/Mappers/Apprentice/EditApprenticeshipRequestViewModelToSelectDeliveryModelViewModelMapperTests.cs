using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    [TestFixture]
    public class EditApprenticeshipRequestViewModelToSelectDeliveryModelViewModelMapperTests
    {
        private EditApprenticeshipRequestViewModelToSelectDeliveryModelViewModelMapper _mapper;
        private EditApprenticeshipRequestViewModel _source;
        private Mock<ICommitmentsApiClient> _commitmentsApiClient;
        private Mock<IApprovalsApiClient> _approvalsApiClient;
        private GetCohortResponse _getCohortResponse;
        private GetApprenticeshipResponse _getApprenticeshipResponse;
        private List<TrainingProgramme> _standardTrainingProgrammes;
        private List<TrainingProgramme> _allTrainingProgrammes;
        private ProviderCourseDeliveryModels _providerCourseDeliveryModels;
        private SelectDeliveryModelViewModel _result;
        private long _cohortId;
        private Fixture _autoFixture;

        [SetUp]
        public async Task Arrange()
        {
            _autoFixture = new Fixture();
            _cohortId = _autoFixture.Create<long>();

            _standardTrainingProgrammes = _autoFixture.CreateMany<TrainingProgramme>().ToList();
            _allTrainingProgrammes = _autoFixture.CreateMany<TrainingProgramme>().ToList();
            _providerCourseDeliveryModels = _autoFixture.Create<ProviderCourseDeliveryModels>();

            _getApprenticeshipResponse = _autoFixture.Build<GetApprenticeshipResponse>()
                .With(x => x.CohortId, _cohortId)
                .Create();

            _getCohortResponse = _autoFixture.Build<GetCohortResponse>()
                .With(x => x.LevyStatus, ApprenticeshipEmployerType.Levy)
                .With(x => x.WithParty, Party.Employer)
                .Without(x => x.TransferSenderId)
                .Create();

            _source = _autoFixture.Build<EditApprenticeshipRequestViewModel>()
                .With(x=>x.DateOfBirth, new DateModel())
                .With(x=>x.StartDate, new MonthYearModel(""))
                .With(x=>x.EndDate, new MonthYearModel(""))
                .With(x=>x.EmploymentEndDate, new MonthYearModel(""))
                .With(x => x.DeliveryModel, DeliveryModel.PortableFlexiJob)
                .Create();

            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();

            _commitmentsApiClient.Setup(x => x.GetApprenticeship(_source.ApprenticeshipId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_getApprenticeshipResponse);
            _commitmentsApiClient.Setup(x => x.GetCohort(_getApprenticeshipResponse.CohortId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_getCohortResponse);
            _commitmentsApiClient.Setup(x => x.GetAllTrainingProgrammeStandards(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetAllTrainingProgrammeStandardsResponse()
                {
                    TrainingProgrammes = _standardTrainingProgrammes
                });
            _commitmentsApiClient
                .Setup(x => x.GetAllTrainingProgrammes(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetAllTrainingProgrammesResponse
                {
                    TrainingProgrammes = _allTrainingProgrammes
                });

            _approvalsApiClient = new Mock<IApprovalsApiClient>();
            _approvalsApiClient.Setup(x => x.GetProviderCourseDeliveryModels(_getCohortResponse.ProviderId.Value, _source.CourseCode,
                    It.IsAny<CancellationToken>())).ReturnsAsync(_providerCourseDeliveryModels);

            _mapper = new EditApprenticeshipRequestViewModelToSelectDeliveryModelViewModelMapper(_commitmentsApiClient.Object, _approvalsApiClient.Object);

            _result = await _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public void DeliveryModelMappedCorrectly()
        {
            Assert.AreEqual(_source.DeliveryModel, _result.DeliveryModel);
        }

        [Test]
        public void DeliveryModelsAreCorrectlyMapped()
        {
            Assert.AreEqual(_providerCourseDeliveryModels.DeliveryModels.ToArray(), _result.DeliveryModels);
        }
    }
}

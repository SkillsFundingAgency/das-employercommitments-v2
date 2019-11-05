using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class ApprenticeViewModelMapperTests
    {
        private ApprenticeViewModelMapper _mapper;
        private Mock<ICommitmentsApiClient> _commitmentsApiClient;
        private GetProviderResponse _providerResponse;
        private ApprenticeRequest _source;
        private ApprenticeViewModel _result;
        private IReadOnlyList<ITrainingProgramme> _courses;
        private Mock<ITrainingProgrammeApiClient> _trainingProgrammeApiClient;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _providerResponse = autoFixture.Create<GetProviderResponse>();
            _source = autoFixture.Create<ApprenticeRequest>();
            _source.StartMonthYear = "062020";

            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _commitmentsApiClient.Setup(x => x.GetProvider(It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_providerResponse);

            _courses = autoFixture.Create<List<Standard>>();
            _trainingProgrammeApiClient = new Mock<ITrainingProgrammeApiClient>();
            _trainingProgrammeApiClient.Setup(x => x.GetAllTrainingProgrammes()).ReturnsAsync(_courses);

            _mapper = new ApprenticeViewModelMapper(
                _commitmentsApiClient.Object,
                _trainingProgrammeApiClient.Object);

            _result = await _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public void AccountLegalEntityIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.AccountLegalEntityId, _result.AccountLegalEntityId);
        }

        [Test]
        public void AccountLegalEntityHashedIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.AccountLegalEntityHashedId, _result.AccountLegalEntityHashedId);
        }

        [Test]
        public void StartDateIsMappedCorrectly()
        {
            Assert.AreEqual(new MonthYearModel(_source.StartMonthYear).Date, _result.StartDate.Date);
        }

        [Test]
        public void ReservationIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.ReservationId, _result.ReservationId);
        }

        [Test]
        public void CourseCodeIsMappedCorrectly()
        {
            Assert.AreEqual(_source.CourseCode, _result.CourseCode);
        }

        [Test]
        public void ProviderIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.ProviderId, _result.ProviderId);
        }

        [Test]
        public void ProviderNameIsMappedCorrectly()
        {
            Assert.AreEqual(_providerResponse.Name, _result.ProviderName);
        }

        [Test]
        public void CoursesAreMappedCorrectly()
        {
            Assert.AreEqual(_courses, _result.Courses);
        }

    }
}

﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class CreateCohortWithDraftApprenticeshipRequestToAddDraftApprenticeshipViewModelMapperTests
    {
        private AddDraftApprenticeshipViewModelMapper _mapper;
        private Mock<ICommitmentsApiClient> _commitmentsApiClient;
        private GetProviderResponse _providerResponse;
        private AccountLegalEntityResponse _accountLegalEntityResponse;
        private ApprenticeRequest _source;
        private AddDraftApprenticeshipViewModel _result;
        private List<TrainingProgramme> _standardTrainingProgrammes;
        private List<TrainingProgramme> _allTrainingProgrammes;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _standardTrainingProgrammes = autoFixture.CreateMany<TrainingProgramme>().ToList();
            _allTrainingProgrammes = autoFixture.CreateMany<TrainingProgramme>().ToList();
            _providerResponse = autoFixture.Create<GetProviderResponse>();
            _accountLegalEntityResponse = autoFixture.Build<AccountLegalEntityResponse>().With(x => x.LevyStatus, ApprenticeshipEmployerType.Levy).Create();

            _source = autoFixture.Build<ApprenticeRequest>()
                .With(x=>x.StartMonthYear, "062020")
                .With(x=>x.AccountId, 12345)
                .With(x => x.ShowTrainingDetails, true)
                .Without(x=>x.TransferSenderId).Create();

            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _commitmentsApiClient.Setup(x => x.GetProvider(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_providerResponse);
            _commitmentsApiClient.Setup(x => x.GetAccountLegalEntity(_source.AccountLegalEntityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_accountLegalEntityResponse);
            _commitmentsApiClient
                .Setup(x => x.GetAllTrainingProgrammeStandards(It.IsAny<CancellationToken>()))
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

            _mapper = new AddDraftApprenticeshipViewModelMapper(_commitmentsApiClient.Object);

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
            Assert.AreEqual(_allTrainingProgrammes, _result.Courses);
        }

        [Test]
        public void ShowTrainingDetailsMappedCorrectly()
        {
            Assert.AreEqual(true, _result.ShowTrainingDetails);
        }

        [Test]
        public async Task TransferFundedCohortsAllowStandardCoursesOnlyWhenEmployerIsLevy()
        {
            _source.TransferSenderId = "test";
            _result = await _mapper.Map(TestHelper.Clone(_source));
            _result.Courses.Should().BeEquivalentTo(_standardTrainingProgrammes);
        }

        [TestCase("12345")]
        [TestCase(null)]
        public async Task NonLevyCohortsAllowStandardCoursesOnlyRegardlessOfTransferStatus(string transferSenderId)
        {
            _source.TransferSenderId = transferSenderId;
            _accountLegalEntityResponse.LevyStatus = ApprenticeshipEmployerType.NonLevy;
            
            
            _result = await _mapper.Map(TestHelper.Clone(_source));
            _result.Courses.Should().BeEquivalentTo(_standardTrainingProgrammes);
        }

    }
}

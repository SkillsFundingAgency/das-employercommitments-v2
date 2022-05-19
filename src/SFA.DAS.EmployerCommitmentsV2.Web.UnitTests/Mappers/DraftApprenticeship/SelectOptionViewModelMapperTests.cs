using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship
{
    public class SelectOptionViewModelMapperTests
    {
        private SelectOptionRequest _request;

        private GetDraftApprenticeshipResponse _getDraftApprenticeshipResponse;
        private GetTrainingProgrammeResponse _GetTrainingProgrammeResponse;

        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

        private SelectOptionViewModelMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();

            _request = fixture.Build<SelectOptionRequest>().Create();

            _getDraftApprenticeshipResponse = fixture.Build<GetDraftApprenticeshipResponse>()
                .With(x => x.HasStandardOptions, true)
                .Create();

            _GetTrainingProgrammeResponse = fixture.Build<GetTrainingProgrammeResponse>().Create();

            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            _mockCommitmentsApiClient.Setup(client => client.GetDraftApprenticeship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_getDraftApprenticeshipResponse);

            _mockCommitmentsApiClient.Setup(client => client.GetTrainingProgrammeVersionByStandardUId(_getDraftApprenticeshipResponse.StandardUId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_GetTrainingProgrammeResponse);

            _mapper = new SelectOptionViewModelMapper(_mockCommitmentsApiClient.Object);
        }

        [Test]
        public async Task When_ApprenticeshipVersionHasOptions_Then_ReturnViewModel()
        {
            var result = await _mapper.Map(_request);

            result.DraftApprenticeshipId.Should().Be(_getDraftApprenticeshipResponse.Id);
            result.CohortId.Should().Be(_request.CohortId);
            result.CohortReference.Should().Be(_request.CohortReference);
            result.Options.Should().BeEquivalentTo(_GetTrainingProgrammeResponse.TrainingProgramme.Options);
            result.StandardTitle.Should().Be(_getDraftApprenticeshipResponse.TrainingCourseName);
            result.CourseOption.Should().Be(_getDraftApprenticeshipResponse.TrainingCourseOption);
            result.StandardUrl.Should().Be(_GetTrainingProgrammeResponse.TrainingProgramme.StandardPageUrl);
            result.DeliveryModel.Should().Be(_getDraftApprenticeshipResponse.DeliveryModel);
            result.Cost.Should().Be(_getDraftApprenticeshipResponse.Cost);
            result.EmploymentPrice.Should().Be(_getDraftApprenticeshipResponse.EmploymentPrice);
            result.EmploymentEndDate.MonthYear.Should().Be(_getDraftApprenticeshipResponse.EmploymentEndDate.Value.ToString("MMyyyy"));
        }

        [Test]
        public async Task When_ApprenticeshipVersionDoesNotHaveAnyOptions_Then_ReturnNull()
        {
            _getDraftApprenticeshipResponse.HasStandardOptions = false;

            var result = await _mapper.Map(_request);

            result.Should().BeNull();
        }
    }
}

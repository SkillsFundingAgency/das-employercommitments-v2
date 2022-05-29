using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship
{
    public class SelectOptionToUpdateDraftApprenticeshipRequestMapperTests
    {
        private SelectOptionViewModel _viewModel;
        private GetDraftApprenticeshipResponse _getDraftApprenticeshipResponse;
        private SelectOptionViewModelToUpdateDraftApprenticeshipRequestMapper _mapper;

        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();

            var birthDate = fixture.Create<DateTime?>();
            var startDate = fixture.Create<DateTime?>();
            var endDate = fixture.Create<DateTime?>();

            _viewModel = fixture.Build<SelectOptionViewModel>()
                .Without(x => x.StartDate)
                .Create();

            _getDraftApprenticeshipResponse = fixture.Create<GetDraftApprenticeshipResponse>();

            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            _mockCommitmentsApiClient.Setup(c => c.GetDraftApprenticeship(_viewModel.CohortId.Value, _viewModel.DraftApprenticeshipId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_getDraftApprenticeshipResponse);

            _mapper = new SelectOptionViewModelToUpdateDraftApprenticeshipRequestMapper(_mockCommitmentsApiClient.Object);
        }

        [Test]
        public async Task Then_ViewModelValuesAreMapped()
        {
            var result = await _mapper.Map(_viewModel);

            result.FirstName.Should().Be(_getDraftApprenticeshipResponse.FirstName);
            result.LastName.Should().Be(_getDraftApprenticeshipResponse.LastName);
            result.Email.Should().Be(_getDraftApprenticeshipResponse.Email);
            result.Uln.Should().Be(_getDraftApprenticeshipResponse.Uln);
            result.DateOfBirth.Should().Be(_getDraftApprenticeshipResponse.DateOfBirth);
            result.StartDate.Should().Be(_getDraftApprenticeshipResponse.StartDate);
            result.EndDate.Should().Be(_getDraftApprenticeshipResponse.EndDate);
            result.Reference.Should().Be(_getDraftApprenticeshipResponse.Reference);
            result.ReservationId.Should().Be(_getDraftApprenticeshipResponse.ReservationId);
            result.CourseOption.Should().Be(_viewModel.CourseOption);
            result.Cost.Should().Be(_getDraftApprenticeshipResponse.Cost);
            result.EmploymentPrice.Should().Be(_getDraftApprenticeshipResponse.EmploymentPrice);
            result.EmploymentEndDate.Value.Should().Be(_getDraftApprenticeshipResponse.EmploymentEndDate.Value);
            result.DeliveryModel.Should().Be(_getDraftApprenticeshipResponse.DeliveryModel);
        }

        [Test]
        public async Task And_ChooseLaterIsSelected_Then_EmptyStringIsMappedTooCourseOption()
        {
            _viewModel.CourseOption = "N/A";

            var result = await _mapper.Map(_viewModel);

            result.CourseOption.Should().BeEmpty();
        }
    }
}

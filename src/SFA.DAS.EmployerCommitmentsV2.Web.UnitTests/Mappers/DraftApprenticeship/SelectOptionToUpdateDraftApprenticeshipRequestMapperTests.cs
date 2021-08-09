using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship
{
    public class SelectOptionToUpdateDraftApprenticeshipRequestMapperTests
    {
        private SelectOptionViewModel _viewModel;
        private SelectOptionViewModelToUpdateDraftApprenticeshipRequestMapper _mapper;
    
        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();

            var birthDate = fixture.Create<DateTime?>();
            var startDate = fixture.Create<DateTime?>();
            var endDate = fixture.Create<DateTime?>();

            _viewModel = fixture.Build<SelectOptionViewModel>()
                .With(x => x.BirthDay, birthDate?.Day)
                .With(x => x.BirthMonth, birthDate?.Month)
                .With(x => x.BirthYear, birthDate?.Year)
                .With(x => x.EndMonth, endDate?.Month)
                .With(x => x.EndYear, endDate?.Year)
                .With(x => x.StartMonth, startDate?.Month)
                .With(x => x.StartYear, startDate?.Year)
                .Without(x => x.StartDate)
                .Create();

            _mapper = new SelectOptionViewModelToUpdateDraftApprenticeshipRequestMapper();
        }

        [Test]
        public async Task Then_ViewModelValuesAreMapped()
        {
            var result = await _mapper.Map(_viewModel);

            result.FirstName.Should().Be(_viewModel.FirstName);
            result.LastName.Should().Be(_viewModel.LastName);
            result.Email.Should().Be(_viewModel.Email);
            result.Uln.Should().Be(_viewModel.Uln);
            result.DateOfBirth.Should().Be(_viewModel.DateOfBirth.Date);
            result.StartDate.Should().Be(_viewModel.StartDate.Date);
            result.EndDate.Should().Be(_viewModel.EndDate.Date);
            result.Reference.Should().Be(_viewModel.Reference);
            result.ReservationId.Should().Be(_viewModel.ReservationId);
            result.CourseOption.Should().Be(_viewModel.CourseOption);
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

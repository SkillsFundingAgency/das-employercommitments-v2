using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers
{
    public class WhenMappingIndexRequestToViewModel
    {
        [Test, AutoData]
        public void Then_Maps_AccountHashedId(
            IndexRequest request,
            IndexViewModelMapper mapper)
        {
            var viewModel = mapper.Map(request);

            viewModel.AccountHashedId.Should().Be(request.AccountHashedId);
        }

        [Test, AutoData]
        public void Then_Maps_EmployerAccountLegalEntityPublicHashedId(
            IndexRequest request,
            IndexViewModelMapper mapper)
        {
            var viewModel = mapper.Map(request);

            viewModel.AccountLegalEntityHashedId.Should().Be(request.AccountLegalEntityHashedId);
        }

        [Test, AutoData]
        public void Then_Maps_ReservationId(
            IndexRequest request,
            IndexViewModelMapper mapper)
        {
            var viewModel = mapper.Map(request);

            viewModel.ReservationId.Should().Be(request.ReservationId);
        }

        [Test, AutoData]
        public void Then_Maps_StartMonthYear(
            IndexRequest request,
            IndexViewModelMapper mapper)
        {
            var viewModel = mapper.Map(request);

            viewModel.StartMonthYear.Should().Be(request.StartMonthYear);
        }

        [Test, AutoData]
        public void Then_Maps_CourseCode(
            IndexRequest request,
            IndexViewModelMapper mapper)
        {
            var viewModel = mapper.Map(request);

            viewModel.CourseCode.Should().Be(request.CourseCode);
        }
    }
}
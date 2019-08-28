using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers
{
    public class WhenMappingIndexRequestToViewModel
    {
        [Test, AutoData]
        public async Task Then_Maps_AccountHashedId(
            IndexRequest request,
            IndexViewModelMapper mapper)
        {
            var viewModel = await mapper.Map(request);

            viewModel.AccountHashedId.Should().Be(request.AccountHashedId);
        }

        [Test, AutoData]
        public async Task Then_Maps_EmployerAccountLegalEntityPublicHashedId(
            IndexRequest request,
            IndexViewModelMapper mapper)
        {
            var viewModel = await mapper.Map(request);

            viewModel.AccountLegalEntityHashedId.Should().Be(request.AccountLegalEntityHashedId);
        }

        [Test, AutoData]
        public async Task Then_Maps_ReservationId(
            IndexRequest request,
            IndexViewModelMapper mapper)
        {
            var viewModel = await mapper.Map(request);

            viewModel.ReservationId.Should().Be(request.ReservationId);
        }

        [Test, AutoData]
        public async Task Then_Maps_StartMonthYear(
            IndexRequest request,
            IndexViewModelMapper mapper)
        {
            var viewModel = await mapper.Map(request);

            viewModel.StartMonthYear.Should().Be(request.StartMonthYear);
        }

        [Test, AutoData]
        public async Task Then_Maps_CourseCode(
            IndexRequest request,
            IndexViewModelMapper mapper)
        {
            var viewModel = await mapper.Map(request);

            viewModel.CourseCode.Should().Be(request.CourseCode);
        }
    }
}
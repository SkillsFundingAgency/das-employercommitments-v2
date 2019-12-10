using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    public class WhenMappingAssignRequestToViewModel
    {
        [Test, AutoData]
        public async Task Then_Maps_AccountHashedId(
            AssignRequest request,
            AssignViewModelMapper mapper)
        {
            var viewModel = await mapper.Map(request);

            viewModel.AccountHashedId.Should().Be(request.AccountHashedId);
        }

        [Test, AutoData]
        public async Task Then_Maps_EmployerAccountLegalEntityPublicHashedId(
            AssignRequest request,
            AssignViewModelMapper mapper)
        {
            var viewModel = await mapper.Map(request);

            viewModel.AccountLegalEntityHashedId.Should().Be(request.AccountLegalEntityHashedId);
        }

        [Test, AutoData]
        public async Task Then_Maps_ReservationId(
            AssignRequest request,
            AssignViewModelMapper mapper)
        {
            var viewModel = await mapper.Map(request);

            viewModel.ReservationId.Should().Be(request.ReservationId);
        }

        [Test, AutoData]
        public async Task Then_Maps_StartMonthYear(
            AssignRequest request,
            AssignViewModelMapper mapper)
        {
            var viewModel = await mapper.Map(request);

            viewModel.StartMonthYear.Should().Be(request.StartMonthYear);
        }

        [Test, AutoData]
        public async Task Then_Maps_CourseCode(
            AssignRequest request,
            AssignViewModelMapper mapper)
        {
            var viewModel = await mapper.Map(request);

            viewModel.CourseCode.Should().Be(request.CourseCode);
        }

        [Test, AutoData]
        public async Task Then_Maps_UkPrn(
            AssignRequest request,
            AssignViewModelMapper mapper)
        {
            var viewModel = await mapper.Map(request);

            viewModel.ProviderId.Should().Be(request.ProviderId);
        }

        [Test, AutoData]
        public async Task Then_Maps_TransferSenderId(
            AssignRequest request,
            AssignViewModelMapper mapper)
        {
            var viewModel = await mapper.Map(request);

            viewModel.TransferSenderId.Should().Be(request.TransferSenderId);
        }
    }
}
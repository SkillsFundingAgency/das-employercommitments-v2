using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers
{
    public class WhenMappingConfirmProviderRequestToViewModel
    {
        [Test, AutoData]
        public void Then_Maps_AccountHashedId(
            ConfirmProviderRequest request,
            ConfirmProviderViewModelMapper mapper)
        {
            var viewModel = mapper.Map(request);

            viewModel.AccountHashedId.Should().Be(request.AccountHashedId);
        }

        [Test, AutoData]
        public void Then_Maps_EmployerAccountLegalEntityPublicHashedId(
            ConfirmProviderRequest request,
            ConfirmProviderViewModelMapper mapper)
        {
            var viewModel = mapper.Map(request);

            viewModel.AccountLegalEntityHashedId.Should().Be(request.AccountLegalEntityHashedId);
        }

        [Test, AutoData]
        public void Then_Maps_ReservationId(
            ConfirmProviderRequest request,
            ConfirmProviderViewModelMapper mapper)
        {
            var viewModel = mapper.Map(request);

            viewModel.ReservationId.Should().Be(request.ReservationId);
        }

        [Test, AutoData]
        public void Then_Maps_StartMonthYear(
            ConfirmProviderRequest request,
            ConfirmProviderViewModelMapper mapper)
        {
            var viewModel = mapper.Map(request);

            viewModel.StartMonthYear.Should().Be(request.StartMonthYear);
        }

        [Test, AutoData]
        public void Then_Maps_CourseCode(
            ConfirmProviderRequest request,
            ConfirmProviderViewModelMapper mapper)
        {
            var viewModel = mapper.Map(request);

            viewModel.CourseCode.Should().Be(request.CourseCode);
        }

        [Test, AutoData]
        public void Then_Maps_ProviderId(
            ConfirmProviderRequest request,
            ConfirmProviderViewModelMapper mapper)
        {
            var viewModel = mapper.Map(request);

            viewModel.ProviderId.Should().Be(request.ProviderId);
        }
    }
}
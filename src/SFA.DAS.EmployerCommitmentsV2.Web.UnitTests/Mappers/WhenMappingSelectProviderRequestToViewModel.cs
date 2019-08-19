using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers
{
    [TestFixture]
    public class WhenMappingSelectProviderRequestToViewModel
    {

        [Test, AutoData]
        public void ThenMapsReservationId(
            SelectProviderRequest request,
            SelectProviderViewModelMapper mapper)
        {
            var result = mapper.Map(request);

            Assert.AreEqual(request.ReservationId, result.ReservationId);
        }

        [Test, MoqAutoData]
        public void ThenMapsAccountHashedId(
            SelectProviderRequest request,
            SelectProviderViewModelMapper mapper)
        {
            var result = mapper.Map(request);

            Assert.AreEqual(request.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public void ThenMapsCourseCode(
            SelectProviderRequest request,
            SelectProviderViewModelMapper mapper)
        {
            var result = mapper.Map(request);

            Assert.AreEqual(request.CourseCode, result.CourseCode);
        }

        [Test, MoqAutoData]
        public void ThenMapsStartMonthYear(
            SelectProviderRequest request,
            SelectProviderViewModelMapper mapper)
        {
            var result = mapper.Map(request);

            Assert.AreEqual(request.StartMonthYear, result.StartMonthYear);
        }

        [Test, MoqAutoData]
        public void ThenMapsEmployerAccountLegalEntityPublicHashedId(
            SelectProviderRequest request,
            SelectProviderViewModelMapper mapper)
        {
            var result = mapper.Map(request);

            Assert.AreEqual(request.AccountLegalEntityHashedId, result.AccountLegalEntityHashedId);
        }
    }
}

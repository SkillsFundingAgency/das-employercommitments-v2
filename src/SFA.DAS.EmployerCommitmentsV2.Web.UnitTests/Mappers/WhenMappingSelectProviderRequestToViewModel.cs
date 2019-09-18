using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers
{
    [TestFixture]
    public class WhenMappingSelectProviderRequestToViewModel
    {

        [Test, MoqAutoData]
        public async Task ThenMapsReservationId(
            SelectProviderRequest request,
            SelectProviderViewModelMapper mapper)
        {
            var result = await mapper.Map(request);

            Assert.AreEqual(request.ReservationId, result.ReservationId);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsAccountHashedId(
            SelectProviderRequest request,
            SelectProviderViewModelMapper mapper)
        {
            var result = await mapper.Map(request);

            Assert.AreEqual(request.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsCourseCode(
            SelectProviderRequest request,
            SelectProviderViewModelMapper mapper)
        {
            var result = await mapper.Map(request);

            Assert.AreEqual(request.CourseCode, result.CourseCode);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsStartMonthYear(
            SelectProviderRequest request,
            SelectProviderViewModelMapper mapper)
        {
            var result = await mapper.Map(request);

            Assert.AreEqual(request.StartMonthYear, result.StartMonthYear);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsEmployerAccountLegalEntityPublicHashedId(
            SelectProviderRequest request,
            SelectProviderViewModelMapper mapper)
        {
            var result = await mapper.Map(request);

            Assert.AreEqual(request.AccountLegalEntityHashedId, result.AccountLegalEntityHashedId);
        }
    }
}

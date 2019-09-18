using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers
{
    public class WhenMappingConfirmProviderViewModelToAssignRequest
    {
        [Test, MoqAutoData]
        public async Task ThenMapsReservationId(
            ConfirmProviderViewModel request,
            AssignRequestMapper mapper)
        {
            var result = await mapper.Map(request);

            Assert.AreEqual(request.ReservationId, result.ReservationId);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsAccountHashedId(
            ConfirmProviderViewModel request,
            AssignRequestMapper mapper)
        {
            var result = await mapper.Map(request);

            Assert.AreEqual(request.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsCourseCode(
            ConfirmProviderViewModel request,
            AssignRequestMapper mapper)
        {
            var result = await mapper.Map(request);

            Assert.AreEqual(request.CourseCode, result.CourseCode);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsStartMonthYear(
            ConfirmProviderViewModel request,
            AssignRequestMapper mapper)
        {
            var result = await mapper.Map(request);

            Assert.AreEqual(request.StartMonthYear, result.StartMonthYear);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsEmployerAccountLegalEntityPublicHashedId(
            ConfirmProviderViewModel request,
            AssignRequestMapper mapper)
        {
            var result = await mapper.Map(request);

            Assert.AreEqual(request.AccountLegalEntityHashedId, result.AccountLegalEntityHashedId);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsProviderId(
            ConfirmProviderViewModel request,
            AssignRequestMapper mapper)
        {
            var result = await mapper.Map(request);

            Assert.AreEqual(request.ProviderId, result.ProviderId);
        }
    }
}

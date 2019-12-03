using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class WhenMappingConfirmProviderRequest
    {
        [Test, MoqAutoData]
        public async Task ThenMapsReservationId(
            SelectProviderViewModel viewModel,
            long providerId,
            ConfirmProviderRequestMapper mapper)
        {
            viewModel.ProviderId = providerId.ToString();

            var result = await mapper.Map(viewModel);

            Assert.AreEqual(viewModel.ReservationId,result.ReservationId);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsCourseCode(
            SelectProviderViewModel viewModel,
            long providerId,
            ConfirmProviderRequestMapper mapper)
        {
            viewModel.ProviderId = providerId.ToString();

            var result = await mapper.Map(viewModel);

            Assert.AreEqual(viewModel.CourseCode, result.CourseCode);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsAccountHashedId(
            SelectProviderViewModel viewModel,
            long providerId,
            ConfirmProviderRequestMapper mapper)
        {
            viewModel.ProviderId = providerId.ToString();

            var result = await mapper.Map(viewModel);

            Assert.AreEqual(viewModel.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsEmployerAccountLegalEntityPublicHashedId(
            SelectProviderViewModel viewModel,
            long providerId,
            ConfirmProviderRequestMapper mapper)
        {
            viewModel.ProviderId = providerId.ToString();

            var result = await mapper.Map(viewModel);

            Assert.AreEqual(viewModel.AccountLegalEntityHashedId, result.AccountLegalEntityHashedId);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsStartMonthYear(
            SelectProviderViewModel viewModel,
            long providerId,
            ConfirmProviderRequestMapper mapper)
        {
            viewModel.ProviderId = providerId.ToString();

            var result = await mapper.Map(viewModel);

            Assert.AreEqual(viewModel.StartMonthYear, result.StartMonthYear);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsProviderId(
            SelectProviderViewModel viewModel,
            long providerId,
            ConfirmProviderRequestMapper mapper)
        {
            viewModel.ProviderId = providerId.ToString();

            var result = await mapper.Map(viewModel);

            Assert.AreEqual(providerId, result.ProviderId);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsTransferSenderId(
            SelectProviderViewModel viewModel,
            long providerId,
            ConfirmProviderRequestMapper mapper)
        {
            viewModel.ProviderId = providerId.ToString();

            var result = await mapper.Map(viewModel);

            Assert.AreEqual(viewModel.TransferSenderId, result.TransferSenderId);
        }
    }
}

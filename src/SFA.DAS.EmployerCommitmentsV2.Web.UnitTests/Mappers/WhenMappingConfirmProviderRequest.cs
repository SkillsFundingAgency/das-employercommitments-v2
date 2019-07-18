using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers
{
    [TestFixture]
    public class WhenMappingConfirmProviderRequest
    {
        [Test, MoqAutoData]
        public void ThenMapsReservationId(
            SelectProviderViewModel viewModel,
            ConfirmProviderRequestMapper mapper)
        {
            var result = mapper.Map(viewModel);

            Assert.AreEqual(viewModel.ReservationId,result.ReservationId);
        }

        [Test, MoqAutoData]
        public void ThenMapsCourseCode(
            SelectProviderViewModel viewModel,
            ConfirmProviderRequestMapper mapper)
        {
            var result = mapper.Map(viewModel);

            Assert.AreEqual(viewModel.CourseCode, result.CourseCode);
        }

        [Test, MoqAutoData]
        public void ThenMapsAccountHashedId(
            SelectProviderViewModel viewModel,
            ConfirmProviderRequestMapper mapper)
        {
            var result = mapper.Map(viewModel);

            Assert.AreEqual(viewModel.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public void ThenMapsEmployerAccountLegalEntityPublicHashedId(
            SelectProviderViewModel viewModel,
            ConfirmProviderRequestMapper mapper)
        {
            var result = mapper.Map(viewModel);

            Assert.AreEqual(viewModel.EmployerAccountLegalEntityPublicHashedId, result.EmployerAccountLegalEntityPublicHashedId);
        }

        [Test, MoqAutoData]
        public void ThenMapsStartMonthYear(
            SelectProviderViewModel viewModel,
            ConfirmProviderRequestMapper mapper)
        {
            var result = mapper.Map(viewModel);

            Assert.AreEqual(viewModel.StartMonthYear, result.StartMonthYear);
        }

        [Test, MoqAutoData]
        public void ThenMapsProviderId(
            SelectProviderViewModel viewModel,
            ConfirmProviderRequestMapper mapper)
        {
            var result = mapper.Map(viewModel);

            Assert.AreEqual(viewModel.ProviderId, result.ProviderId);
        }
    }
}

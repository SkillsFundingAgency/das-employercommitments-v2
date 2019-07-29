using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers
{
    public class WhenMappingSelectProviderViewModelToConfirmProviderViewModel
    {
        [Test, AutoData]
        public void ThenMapsReservationId(
            ConfirmProviderViewModel request,
            SelectProviderConfirmProviderMapper mapper)
        {
            var result = mapper.Map(request);

            Assert.AreEqual(request.ReservationId, result.ReservationId);
        }

        [Test, MoqAutoData]
        public void ThenMapsAccountHashedId(
            ConfirmProviderViewModel request,
            SelectProviderConfirmProviderMapper mapper)
        {
            var result = mapper.Map(request);

            Assert.AreEqual(request.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public void ThenMapsCourseCode(
            ConfirmProviderViewModel request,
            SelectProviderConfirmProviderMapper mapper)
        {
            var result = mapper.Map(request);

            Assert.AreEqual(request.CourseCode, result.CourseCode);
        }

        [Test, MoqAutoData]
        public void ThenMapsStartMonthYear(
            ConfirmProviderViewModel request,
            SelectProviderConfirmProviderMapper mapper)
        {
            var result = mapper.Map(request);

            Assert.AreEqual(request.StartMonthYear, result.StartMonthYear);
        }

        [Test, MoqAutoData]
        public void ThenMapsEmployerAccountLegalEntityPublicHashedId(
            ConfirmProviderViewModel request,
            SelectProviderConfirmProviderMapper mapper)
        {
            var result = mapper.Map(request);

            Assert.AreEqual(request.AccountLegalEntityHashedId, result.AccountLegalEntityHashedId);
        }
    }
}

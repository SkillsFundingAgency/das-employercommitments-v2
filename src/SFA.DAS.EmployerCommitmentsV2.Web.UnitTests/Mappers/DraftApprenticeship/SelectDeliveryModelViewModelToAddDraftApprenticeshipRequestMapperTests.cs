﻿using AutoFixture;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship
{
    [TestFixture]
    public class SelectDeliveryModelViewModelToAddDraftApprenticeshipRequestMapperTests
    {
        private SelectDeliveryModelViewModelToAddDraftApprenticeshipRequestMapper _mapper;
        private AddDraftApprenticeshipRequest _result;
        private SelectDeliveryModelViewModel _source;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _source = autoFixture.Build<SelectDeliveryModelViewModel>().Without(x => x.DeliveryModels).Create();

            _mapper = new SelectDeliveryModelViewModelToAddDraftApprenticeshipRequestMapper();

            _result = await _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public void AccountHashedIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.AccountHashedId, _result.AccountHashedId);
        }

        [Test]
        public void AccountLegalEntityIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.AccountLegalEntityId, _result.AccountLegalEntityId);
        }

        [Test]
        public void AccountLegalEntityHashedIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.AccountLegalEntityHashedId, _result.AccountLegalEntityHashedId);
        }

        [Test]
        public void CohortIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.CohortId, _result.CohortId);
        }

        [Test]
        public void CohortReferenceIsMappedCorrectly()
        {
            Assert.AreEqual(_source.CohortReference, _result.CohortReference);
        }

        [Test]
        public void CourseCodeIsMappedCorrectly()
        {
            Assert.AreEqual(_source.CourseCode, _result.CourseCode);
        }

        [Test]
        public void DeliveryModelIsMappedCorrectly()
        {
            Assert.AreEqual(_source.DeliveryModel, _result.DeliveryModel);
        }

        [Test]
        public void ProviderIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.ProviderId, _result.ProviderId);
        }

        [Test]
        public void ReservationIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.ReservationId, _result.ReservationId);
        }

        [Test]
        public void StartDateIsMappedCorrectly()
        {
            Assert.AreEqual(_source.StartMonthYear, _result.StartMonthYear);
        }
    }
}
using System;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.Commitments.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers
{
    [TestFixture]
    public class WhenIMapEditDraftApprenticeshipDetailsToViewModel
    {
        private EditDraftApprenticeshipDetailsToViewModelMapper _mapper;
        private EditDraftApprenticeshipDetails _source;
        private Func<EditDraftApprenticeshipViewModel> _act;

        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();

            _mapper = new EditDraftApprenticeshipDetailsToViewModelMapper();
            _source = fixture.Build<EditDraftApprenticeshipDetails>().Create();

            _act = () => _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public void ThenDraftApprenticeshipIdIsMappedCorrectly()
        {
            var result = _act();
            Assert.AreEqual(_source.DraftApprenticeshipId, result.DraftApprenticeshipId);
        }

        [Test]
        public void ThenDraftApprenticeshipHashedIdIsMappedCorrectly()
        {
            var result = _act();
            Assert.AreEqual(_source.DraftApprenticeshipId, result.DraftApprenticeshipId);
        }

        [Test]
        public void ThenCohortIdIsMappedCorrectly()
        {
            var result = _act();
            Assert.AreEqual(_source.CohortId, result.CohortId);
        }

        [Test]
        public void ThenCohortReferenceIsMappedCorrectly()
        {
            var result = _act();
            Assert.AreEqual(_source.CohortReference, result.CohortReference);
        }

        [Test]
        public void ThenReservationIdIsMappedCorrectly()
        {
            var result = _act();
            Assert.AreEqual(_source.ReservationId, result.ReservationId);
        }

        [Test]
        public void ThenFirstNameIsMappedCorrectly()
        {
            var result = _act();
            Assert.AreEqual(_source.FirstName, result.FirstName);
        }

        [Test]
        public void ThenLastNameIsMappedCorrectly()
        {
            var result = _act();
            Assert.AreEqual(_source.LastName, result.LastName);
        }

        [Test]
        public void ThenUniqueLearnerNumberIsMappedCorrectly()
        {
            var result = _act();
            Assert.AreEqual(_source.UniqueLearnerNumber, result.Uln);
        }

        [Test]
        public void ThenDateOfBirthIsMappedCorrectly()
        {
            var result = _act();
            Assert.AreEqual(_source.DateOfBirth?.Day, result.DateOfBirth.Day);
            Assert.AreEqual(_source.DateOfBirth?.Month, result.DateOfBirth.Month);
            Assert.AreEqual(_source.DateOfBirth?.Year, result.DateOfBirth.Year);
        }

        [Test]
        public void ThenCourseCodeIsMappedCorrectly()
        {
            var result = _act();
            Assert.AreEqual(_source.CourseCode, result.CourseCode);
        }

        [Test]
        public void ThenCostIsMappedCorrectly()
        {
            var result = _act();
            Assert.AreEqual(_source.Cost, result.Cost);
        }

        [Test]
        public void ThenStartDateIsMappedCorrectly()
        {
            var result = _act();
            Assert.AreEqual(_source.StartDate?.Month, result.StartDate.Month);
            Assert.AreEqual(_source.StartDate?.Year, result.StartDate.Year);
        }

        [Test]
        public void ThenEndDateIsMappedCorrectly()
        {
            var result = _act();
            Assert.AreEqual(_source.EndDate?.Month, result.EndDate.Month);
            Assert.AreEqual(_source.EndDate?.Year, result.EndDate.Year);
        }

        [Test]
        public void ThenOriginatorReferenceIsMappedCorrectly()
        {
            var result = _act();
            Assert.AreEqual(_source.OriginatorReference, result.Reference);
        }

        [Test]
        public void ThenProviderIdIsMappedCorrectly()
        {
            var result = _act();
            Assert.AreEqual(_source.ProviderId, result.ProviderId);
        }
    }
}
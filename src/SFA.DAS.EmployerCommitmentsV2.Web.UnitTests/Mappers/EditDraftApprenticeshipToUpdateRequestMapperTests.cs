using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers
{
    [TestFixture]
    public class WhenIMapDraftApprenticeshipToUpdateRequest
    {
        private EditDraftApprenticeshipToUpdateRequestMapper _mapper;
        private EditDraftApprenticeshipViewModel _source;
        private Func<UpdateDraftApprenticeshipRequest> _act;
        public Mock<IAuthenticationService> AuthenticationServiceMock;

        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();

            var birthDate = fixture.Create<DateTime?>();
            var startDate = fixture.Create<DateTime?>();
            var endDate = fixture.Create<DateTime?>();
            AuthenticationServiceMock = new Mock<IAuthenticationService>();

            _mapper = new EditDraftApprenticeshipToUpdateRequestMapper(AuthenticationServiceMock.Object);

            _source = fixture.Build<EditDraftApprenticeshipViewModel>()
                .With(x => x.BirthDay, birthDate?.Day)
                .With(x => x.BirthMonth, birthDate?.Month)
                .With(x => x.BirthYear, birthDate?.Year)
                .With(x => x.EndMonth, endDate?.Month)
                .With(x => x.EndYear, endDate?.Year)
                .With(x => x.StartMonth, startDate?.Month)
                .With(x => x.StartYear, startDate?.Year)
                .Without(x => x.StartDate)
                .Without(x => x.Courses)
                .Create();

            _act = () => _mapper.Map(TestHelper.Clone(_source));
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
        public void ThenDateOfBirthIsMappedCorrectly()
        {
            var result = _act();
            Assert.AreEqual(_source.DateOfBirth.Date, result.DateOfBirth);
        }

        [Test]
        public void ThenUniqueLearnerNumberIsMappedToNull()
        {
            var result = _act();
            Assert.AreEqual(_source.Uln, result.Uln);
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
            Assert.AreEqual(_source.StartDate.Date, result.StartDate);
        }

        [Test]
        public void ThenEndDateIsMappedCorrectly()
        {
            var result = _act();
            Assert.AreEqual(_source.EndDate.Date, result.EndDate);
        }

        [Test]
        public void ThenOriginatorReferenceIsMappedCorrectly()
        {
            var result = _act();
            Assert.AreEqual(_source.Reference, result.Reference);
        }

        [Test]
        public void AndWhenUserIsNotAuthenticated_ThenUserInfoIsNull()
        {
            AuthenticationServiceMock.Setup(x => x.IsUserAuthenticated()).Returns(false);
            var result = _act();

            Assert.IsNull(result.UserInfo);
        }

        [Test]
        public void AndWhenUserIsAuthenticated_ThenUserInfoMatchesAuthenticationService()
        {
            AuthenticationServiceMock.Setup(x => x.IsUserAuthenticated()).Returns(true);
            AuthenticationServiceMock.Setup(x => x.UserId).Returns("Id");
            AuthenticationServiceMock.Setup(x => x.UserName).Returns("Name");
            AuthenticationServiceMock.Setup(x => x.UserEmail).Returns("Email");

            var result = _act();

            Assert.IsNotNull(result.UserInfo);
            Assert.AreEqual("Id", result.UserInfo.UserId);
            Assert.AreEqual("Name", result.UserInfo.UserDisplayName);
            Assert.AreEqual("Email", result.UserInfo.UserEmail);
        }

    }
}
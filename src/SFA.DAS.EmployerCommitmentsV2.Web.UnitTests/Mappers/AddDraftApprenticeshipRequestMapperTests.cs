using System;
using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers
{
    [TestFixture]
    public class WhenIMapDraftApprenticeshipRequest
    {

        private AddDraftApprenticeshipRequestMapper _mapper;
        private AddDraftApprenticeshipViewModel _source;
        private CreateCohortRequest _result;
        public Mock<IAuthenticationService> AuthenticationServiceMock;

        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();

            var birthDate = fixture.Create<DateTime?>();
            var startDate = fixture.Create<DateTime?>();
            var endDate = fixture.Create<DateTime?>();
            AuthenticationServiceMock = new Mock<IAuthenticationService>();

            _mapper = new AddDraftApprenticeshipRequestMapper(AuthenticationServiceMock.Object);

            _source = fixture.Build<AddDraftApprenticeshipViewModel>()
                .With(x => x.BirthDay, birthDate?.Day)
                .With(x => x.BirthMonth, birthDate?.Month)
                .With(x => x.BirthYear, birthDate?.Year)
                .With(x => x.Cost, birthDate?.Year)
                .With(x => x.EndMonth, endDate?.Month)
                .With(x => x.EndYear, endDate?.Year)
                .With(x => x.StartMonth, startDate?.Month)
                .With(x => x.StartYear, startDate?.Year)
                .Without(x => x.StartDate)
                .Without(x => x.Courses)
                .Create();

            _result = _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public void ThenReservationIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.ReservationId, _result.ReservationId);
        }

        [Test]
        public void ThenFirstNameIsMappedCorrectly()
        {
            Assert.AreEqual(_source.FirstName, _result.FirstName);
        }

        [Test]
        public void ThenDateOfBirthIsMappedCorrectly()
        {
            Assert.AreEqual(_source.DateOfBirth.Date, _result.DateOfBirth);
        }

        [Test]
        public void ThenUniqueLearnerNumberIsMappedCorrectly()
        {
            Assert.AreEqual(_source.Uln, _result.Uln);
        }

        [Test]
        public void ThenCourseCodeIsMappedCorrectly()
        {
            Assert.AreEqual(_source.CourseCode, _result.CourseCode);
        }

        [Test]
        public void ThenCostIsMappedCorrectly()
        {
            Assert.AreEqual(_source.Cost, _result.Cost);
        }

        [Test]
        public void ThenStartDateIsMappedCorrectly()
        {
            Assert.AreEqual(_source.StartDate.Date, _result.StartDate);
        }

        [Test]
        public void ThenEndDateIsMappedCorrectly()
        {
            Assert.AreEqual(_source.EndDate.Date, _result.EndDate);
        }

        [Test]
        public void ThenOriginatorReferenceIsMappedCorrectly()
        {
            Assert.AreEqual(_source.Reference, _result.OriginatorReference);
        }

        [Test]
        public void ThenProviderIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.ProviderId, _result.ProviderId);
        }

        [Test]
        public void AndWhenUserIsNotAuthenticated_ThenUserInfoIsNull()
        {
            AuthenticationServiceMock.Setup(x => x.IsUserAuthenticated()).Returns(false);
            _result = _mapper.Map(TestHelper.Clone(_source));

            Assert.IsNull(_result.UserInfo);
        }

        [Test]
        public void AndWhenUserIsAuthenticated_ThenUserInfoMatchesAuthenticationService()
        {
            AuthenticationServiceMock.Setup(x => x.IsUserAuthenticated()).Returns(true);
            AuthenticationServiceMock.Setup(x => x.UserId).Returns("Id");
            AuthenticationServiceMock.Setup(x => x.UserName).Returns("Name");
            AuthenticationServiceMock.Setup(x => x.UserEmail).Returns("Email");

            _result = _mapper.Map(TestHelper.Clone(_source));

            Assert.IsNotNull(_result.UserInfo);
            Assert.AreEqual("Id", _result.UserInfo.UserId);
            Assert.AreEqual("Name", _result.UserInfo.UserDisplayName);
            Assert.AreEqual("Email", _result.UserInfo.UserEmail);
        }
    }
}

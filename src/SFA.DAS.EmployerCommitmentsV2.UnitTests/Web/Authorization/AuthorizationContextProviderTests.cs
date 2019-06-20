using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.UnitTests.Web.Authorization
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationContextProviderTests
    {
        private AuthorizationContextProviderTestsFixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new AuthorizationContextProviderTestsFixture();
        }

        [Test]
        public void GetAuthorizationContext_WhenAccountIdExistsAndIsValid_ThenShouldReturnAuthorizationContextWithAccountIdAndPartyId()
        {
            _fixture.SetUserRef(Guid.NewGuid()).SetValidAccountId();

            var authorizationContext = _fixture.GetAuthorizationContext();

            Assert.IsNotNull(authorizationContext);
            Assert.AreEqual(_fixture.AccountId, authorizationContext.Get<long?>("AccountId"));
            Assert.AreEqual(_fixture.AccountId, authorizationContext.Get<long?>("PartyId"));
        }

        [Test]
        public void GetAuthorizationContext_WhenAccountIdDoesNotExist_ThenShouldReturnAuthorizationContextWithoutAccountId()
        {
            _fixture.SetUserRef(Guid.NewGuid());

            var authorizationContext = _fixture.GetAuthorizationContext();

            Assert.IsNotNull(authorizationContext);
            Assert.IsNull(authorizationContext.Get<long?>("AccountId"));
        }

        [Test]
        public void GetAuthorizationContext_WhenAccountIdExistsAndIsInvalid_ThenShouldThrowUnauthorizedAccessException()
        {
            _fixture.SetInvalidAccountId();

            Assert.Throws<UnauthorizedAccessException>(() => _fixture.GetAuthorizationContext());
        }

        [Test]
        public void GetAuthorizationContext_WhenCohortIdExistsAndIsValid_ThenShouldReturnAuthorizationContextWithCohortId()
        {
            _fixture.SetValidCohortId();

            var authorizationContext = _fixture.GetAuthorizationContext();

            Assert.IsNotNull(authorizationContext);
            Assert.AreEqual(_fixture.CohortId, authorizationContext.Get<long?>("CohortId"));
        }

        [Test]
        public void GetAuthorizationContext_WhenCohortIdExistsAndIsInvalid_ThenShouldThrowUnauthorizedAccessException()
        {
            _fixture.SetInvalidCohortId();

            Assert.Throws<UnauthorizedAccessException>(() => _fixture.GetAuthorizationContext());
        }

        [Test]
        public void GetAuthorizationContext_WhenCohortIdDoesNotExist_ThenShouldReturnAuthorizationContextWithoutCohortId()
        {
            var authorizationContext = _fixture.GetAuthorizationContext();

            Assert.IsNotNull(authorizationContext);
            Assert.IsNull(authorizationContext.Get<long?>("CohortId"));
        }

        [Test]
        public void GetAuthorizationContext_WhenUserIsAuthenticatedAndUserRefExistsAndIsValid_ThenShouldReturnAuthorizationContextWithUserRef()
        {
            _fixture.SetUserRef(Guid.NewGuid()).SetValidAccountId();

            var authorizationContext = _fixture.GetAuthorizationContext();

            Assert.IsNotNull(authorizationContext);
            Assert.AreEqual(_fixture.UserRef, authorizationContext.Get<Guid?>("UserRef"));
        }

        [Test]
        public void GetAuthorizationContext_WhenUserIsNotAuthenticated_ThenShouldReturnAuthorizationContextWithoutUserRef()
        {
            var authorizationContext = _fixture.GetAuthorizationContext();

            Assert.IsNotNull(authorizationContext);
            Assert.IsNull(authorizationContext.Get<Guid?>("UserRef"));
        }

        [Test]
        public void GetAuthorizationContext_WhenUserIsAuthenticatedAndUserRefIsNull_ThenShouldThrowUnauthorizedAccessException()
        {
            _fixture.SetUserRef(null);

            Assert.Throws<UnauthorizedAccessException>(() => _fixture.GetAuthorizationContext());
        }

        [Test]
        public void GetAuthorizationContext_WhenUserIsAuthenticatedAndUserRefIsNotAGuid_ThenShouldThrowUnauthorizedAccessException()
        {
            _fixture.SetInvalidUserRef();

            Assert.Throws<UnauthorizedAccessException>(() => _fixture.GetAuthorizationContext());
        }
    }

    public class AuthorizationContextProviderTestsFixture
    {
        public IAuthorizationContextProvider AuthorizationContextProvider { get; set; }
        public Mock<IHttpContextAccessor> HttpContextAccessor { get; set; }
        public Mock<IAuthenticationService> AuthenticationService { get; set; }
        public Mock<IRoutingFeature> RoutingFeature { get; set; }
        public Mock<IEncodingService> EncodingService { get; set; }
        public string AccountHashedId { get; set; }
        public long AccountId { get; set; }
        public long CohortId { get; set; }
        public string CohortReference { get; set; }
        public Guid? UserRef { get; set; }
        public string UserRefClaimValue { get; set; }

        public AuthorizationContextProviderTestsFixture()
        {
            AuthenticationService = new Mock<IAuthenticationService>();
            HttpContextAccessor = new Mock<IHttpContextAccessor>();
            RoutingFeature = new Mock<IRoutingFeature>();
            EncodingService = new Mock<IEncodingService>();
            HttpContextAccessor.Setup(c => c.HttpContext.Features[typeof(IRoutingFeature)]).Returns(RoutingFeature.Object);
            RoutingFeature.Setup(f => f.RouteData).Returns(new RouteData());
            HttpContextAccessor.Setup(c => c.HttpContext.Request.Query).Returns(new QueryCollection());
            HttpContextAccessor.Setup(c => c.HttpContext.Request.Form).Returns(new FormCollection(new Dictionary<string, StringValues>()));
            UserRef = Guid.NewGuid();

            AuthorizationContextProvider = new AuthorizationContextProvider(HttpContextAccessor.Object, EncodingService.Object, AuthenticationService.Object);
        }

        public IAuthorizationContext GetAuthorizationContext()
        {
            return AuthorizationContextProvider.GetAuthorizationContext();
        }

        public AuthorizationContextProviderTestsFixture SetValidAccountId()
        {
            AccountHashedId = "ABC";
            AccountId = 123;

            var routeData = new RouteData();
            var accountId = AccountId;

            routeData.Values[RouteValueKeys.AccountHashedId] = AccountHashedId;

            RoutingFeature.Setup(f => f.RouteData).Returns(routeData);
            EncodingService.Setup(h => h.TryDecode(AccountHashedId, EncodingType.AccountId, out accountId)).Returns(true);

            return this;
        }

        public AuthorizationContextProviderTestsFixture SetInvalidAccountId()
        {
            AccountHashedId = "AAA";

            var routeData = new RouteData();
            var accountLegalEntityId = 0L;

            routeData.Values[RouteValueKeys.AccountHashedId] = AccountHashedId;

            RoutingFeature.Setup(f => f.RouteData).Returns(routeData);
            EncodingService.Setup(h => h.TryDecode(AccountHashedId, EncodingType.PublicAccountLegalEntityId, out accountLegalEntityId)).Returns(false);

            return this;
        }

        public AuthorizationContextProviderTestsFixture SetValidCohortId()
        {
            CohortReference = "CDE";
            CohortId = 345;

            var routeData = new RouteData();
            var cohortId = CohortId;

            routeData.Values[RouteValueKeys.CohortReference] = CohortReference;

            RoutingFeature.Setup(f => f.RouteData).Returns(routeData);
            EncodingService.Setup(h => h.TryDecode(CohortReference, EncodingType.CohortReference, out cohortId)).Returns(true);

            return this;
        }

        public AuthorizationContextProviderTestsFixture SetInvalidCohortId()
        {
            CohortReference = "BBB";

            var routeData = new RouteData();
            var cohortId = CohortId;

            routeData.Values[RouteValueKeys.CohortReference] = CohortReference;

            RoutingFeature.Setup(f => f.RouteData).Returns(routeData);
            EncodingService.Setup(h => h.TryDecode(CohortReference, EncodingType.CohortReference, out cohortId)).Returns(false);

            return this;
        }

        public AuthorizationContextProviderTestsFixture SetUserRef(Guid? userRef)
        {
            UserRef = userRef;
            UserRefClaimValue = UserRef == null ? null : UserRef.ToString();

            var userIdClaimValue = UserRefClaimValue;

            AuthenticationService.Setup(a => a.IsUserAuthenticated()).Returns(true);
            AuthenticationService.Setup(a => a.TryGetUserClaimValue(EmployeeClaims.Id, out userIdClaimValue)).Returns(true);

            return this;
        }

        public AuthorizationContextProviderTestsFixture SetInvalidUserRef()
        {
            UserRefClaimValue = "XXXXX";

            var userIdClaimValue = UserRefClaimValue;

            AuthenticationService.Setup(a => a.IsUserAuthenticated()).Returns(true);
            AuthenticationService.Setup(a => a.TryGetUserClaimValue(EmployeeClaims.Id, out userIdClaimValue)).Returns(true);

            return this;
        }


        //public AuthorizationContextProviderTestsFixture SetInvalidUkprn()
        //{
        //    UkprnClaimValue = "BBB";

        //    var ukprnClaimValue = UkprnClaimValue;

        //    AuthenticationService.Setup(a => a.IsUserAuthenticated()).Returns(true);
        //    AuthenticationService.Setup(a => a.TryGetUserClaimValue(ProviderClaims.Ukprn, out ukprnClaimValue)).Returns(true);

        //    return this;
        //}

        //public AuthorizationContextProviderTestsFixture SetValidUserEmail()
        //{
        //    UserEmail = "foo@bar.com";

        //    var userEmailClaimValue = UserEmail;

        //    AuthenticationService.Setup(a => a.IsUserAuthenticated()).Returns(true);
        //    AuthenticationService.Setup(a => a.TryGetUserClaimValue(ProviderClaims.Email, out userEmailClaimValue)).Returns(true);

        //    return this;
        //}
    }
}
﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Context;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Authorization
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
        public void GetAuthorizationContext_WhenAccountIdExistsAndIsValid_ThenShouldReturnAuthorizationContextWithAccountId()
        {
            _fixture.SetUserRef(Guid.NewGuid()).SetValidAccountId();

            var authorizationContext = _fixture.GetAuthorizationContext();

            Assert.IsNotNull(authorizationContext);
            Assert.AreEqual(_fixture.AccountId, authorizationContext.Get<long?>("AccountId"));
        }

        [Test]
        public void GetAuthorizationContext_WhenAccountIdDoesNotExist_ThenShouldThrowKeyNotFoundException()
        {
            _fixture.SetUserRef(Guid.NewGuid());

            var authorizationContext = _fixture.GetAuthorizationContext();

            Assert.IsNotNull(authorizationContext);
            Assert.Throws<KeyNotFoundException>(() => authorizationContext.Get<long?>("AccountId"));
        }

        [Test]
        public void GetAuthorizationContext_WhenAccountIdExistsAndIsInvalid_ThenShouldThrowUnauthorizedAccessException()
        {
            _fixture.SetInvalidAccountId();

            Assert.Throws<UnauthorizedAccessException>(() => _fixture.GetAuthorizationContext());
        }

        [Test]
        public void GetAuthorizationContext_WhenCohortIdAndAccountIdExistAndAreValid_ThenShouldReturnAuthorizationContextWithCohortIdAndPartyId()
        {
            _fixture.SetValidAccountId().SetValidCohortId();

            var authorizationContext = _fixture.GetAuthorizationContext();

            Assert.IsNotNull(authorizationContext);
            Assert.AreEqual(_fixture.CohortId, authorizationContext.Get<long?>("CohortId"));
            Assert.AreEqual(Party.Employer, authorizationContext.Get<Party?>("Party"));
            Assert.AreEqual(_fixture.AccountId, authorizationContext.Get<long?>("PartyId"));
        }

        [Test]
        public void GetAuthorizationContext_WhenAccountIdExistsAndIsValidAndCohortIdExistsAndIsInvalid_ThenShouldThrowUnauthorizedAccessException()
        {
            _fixture.SetValidAccountId().SetInvalidCohortId();

            Assert.Throws<UnauthorizedAccessException>(() => _fixture.GetAuthorizationContext());
        }

        [Test]
        public void GetAuthorizationContext_WhenCohortIdDoesNotExist_ThenShouldThrowKeyNotFoundException()
        {
            var authorizationContext = _fixture.GetAuthorizationContext();

            Assert.IsNotNull(authorizationContext);
            Assert.Throws<KeyNotFoundException>(() => authorizationContext.Get<long?>("CohortId"));
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
        public void GetAuthorizationContext_WhenUserIsNotAuthenticated_ThenShouldThrowKeyNotFoundException()
        {
            var authorizationContext = _fixture.GetAuthorizationContext();

            Assert.IsNotNull(authorizationContext);
            Assert.Throws<KeyNotFoundException>(() => authorizationContext.Get<Guid?>("UserRef"));
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

        [Test]
        public void GetAuthorizationContext_WhenDaftApprenticeshipIdDoesNotExist_ThenShouldThrowKeyNotFoundException()
        {
            _fixture.SetInvalidDraftApprenticeshipId();

            Assert.Throws<UnauthorizedAccessException>(() => _fixture.GetAuthorizationContext());
        }

        [Test]
        public void GetAuthorizationContext_WhenDraftApprenticeshipIdExistAndIsValid_ThenShouldReturnAuthorizationContextWithDraftApprenticeshipId()
        {
            _fixture.SetValidDraftApprenticeshipId();

            var authorizationContext = _fixture.GetAuthorizationContext();

            Assert.IsNotNull(authorizationContext);
            Assert.AreEqual(_fixture.DraftApprenticeshipId, authorizationContext.Get<long?>("DraftApprenticeshipId"));
        }

        [Test]
        public void GetAuthorizationContext_WhenAccountId_And_CohortIdExist_AndIsValid_ThenShouldReturnAuthorizationContextWithEmployerParty()
        {
            _fixture
                .SetValidAccountId()
                .SetValidCohortId()
                .SetUserRef(Guid.NewGuid());

            var authorizationContext = _fixture.GetAuthorizationContext();

            Assert.IsNotNull(authorizationContext);
            Assert.AreEqual(Party.Employer, authorizationContext.Get<Party>("Party"));
        }

        [Test]
        public void GetAuthorizationContext_WhenAccountId_And_ApprenticeshipIdExist_AndIsValid_ThenShouldReturnAuthorizationContextWithEmployerParty()
        {
            _fixture
                .SetValidAccountId()
                .SetValidApprenticeshipId()
                .SetUserRef(Guid.NewGuid());

            var authorizationContext = _fixture.GetAuthorizationContext();

            Assert.IsNotNull(authorizationContext);
            Assert.AreEqual(Party.Employer, authorizationContext.Get<Party>("Party"));
        }
    }

    public class AuthorizationContextProviderTestsFixture
    {
        public IAuthorizationContextProvider AuthorizationContextProvider { get; private set; }
        public Mock<IHttpContextAccessor> HttpContextAccessor { get; private set; }
        public Mock<IAuthenticationService> UserService { get; private set; }
        public Mock<IRoutingFeature> RoutingFeature { get; private set; }
        public Mock<IEncodingService> EncodingService { get; private set; }
        public string DraftApprenticeshipHashedId { get; private set; }
        public long DraftApprenticeshipId { get; private set; }
        public long CohortId { get; private set; }
        public string CohortReference { get; private set; }
        public string AccountHashedId { get; private set; }
        public long AccountId { get; private set; }
        public Guid? UserRef { get; private set; }
        public string UserRefClaimValue { get; private set; }
        public RouteData RouteData { get; private set; }
        public string ApprenticeshipHashedId { get; private set; }
        public long ApprenticeshipId { get; private set; }

        public AuthorizationContextProviderTestsFixture()
        {
            UserService = new Mock<IAuthenticationService>();
            HttpContextAccessor = new Mock<IHttpContextAccessor>();
            RoutingFeature = new Mock<IRoutingFeature>();
            EncodingService = new Mock<IEncodingService>();
            HttpContextAccessor.Setup(c => c.HttpContext.Features[typeof(IRoutingFeature)]).Returns(RoutingFeature.Object);
            RoutingFeature.Setup(f => f.RouteData).Returns(new RouteData());
            HttpContextAccessor.Setup(c => c.HttpContext.Request.Query).Returns(new QueryCollection());
            HttpContextAccessor.Setup(c => c.HttpContext.Request.Form).Returns(new FormCollection(new Dictionary<string, StringValues>()));
            UserRef = Guid.NewGuid();
            RouteData = new RouteData();

            AuthorizationContextProvider = new AuthorizationContextProvider(HttpContextAccessor.Object, EncodingService.Object, UserService.Object);
        }

        public IAuthorizationContext GetAuthorizationContext()
        {
            return AuthorizationContextProvider.GetAuthorizationContext();
        }

        public AuthorizationContextProviderTestsFixture SetValidAccountId()
        {
            AccountHashedId = "ABC";
            AccountId = 123;

            //var routeData = new RouteData();
            var accountId = AccountId;

            RouteData.Values[RouteValueKeys.AccountHashedId] = AccountHashedId;

            RoutingFeature.Setup(f => f.RouteData).Returns(RouteData);
            EncodingService.Setup(h => h.TryDecode(AccountHashedId, EncodingType.AccountId, out accountId)).Returns(true);

            return this;
        }

        public AuthorizationContextProviderTestsFixture SetInvalidAccountId()
        {
            AccountHashedId = "AAA";

            //var routeData = new RouteData();
            var accountLegalEntityId = 0L;

            RouteData.Values[RouteValueKeys.AccountHashedId] = AccountHashedId;

            RoutingFeature.Setup(f => f.RouteData).Returns(RouteData);
            EncodingService.Setup(h => h.TryDecode(AccountHashedId, EncodingType.PublicAccountLegalEntityId, out accountLegalEntityId)).Returns(false);

            return this;
        }

        public AuthorizationContextProviderTestsFixture SetValidCohortId()
        {
            CohortReference = "CDE";
            CohortId = 345;

            //var routeData = new RouteData();
            var cohortId = CohortId;

            RouteData.Values[RouteValueKeys.CohortReference] = CohortReference;

            RoutingFeature.Setup(f => f.RouteData).Returns(RouteData);
            EncodingService.Setup(h => h.TryDecode(CohortReference, EncodingType.CohortReference, out cohortId)).Returns(true);

            return this;
        }

        public AuthorizationContextProviderTestsFixture SetInvalidCohortId()
        {
            CohortReference = "BBB";

            var cohortId = CohortId;

            RouteData.Values[RouteValueKeys.CohortReference] = CohortReference;

            RoutingFeature.Setup(f => f.RouteData).Returns(RouteData);
            EncodingService.Setup(h => h.TryDecode(CohortReference, EncodingType.CohortReference, out cohortId)).Returns(false);

            return this;
        }

        public AuthorizationContextProviderTestsFixture SetValidDraftApprenticeshipId()
        {
            DraftApprenticeshipHashedId = "DEF";
            DraftApprenticeshipId = 989;

            var draftApprenticeshipId = DraftApprenticeshipId;

            RouteData.Values[RouteValueKeys.DraftApprenticeshipHashedId] = DraftApprenticeshipHashedId;

            RoutingFeature.Setup(f => f.RouteData).Returns(RouteData);
            EncodingService.Setup(h => h.TryDecode(DraftApprenticeshipHashedId, EncodingType.ApprenticeshipId, out draftApprenticeshipId)).Returns(true);

            return this;
        }

        public AuthorizationContextProviderTestsFixture SetInvalidDraftApprenticeshipId()
        {
            DraftApprenticeshipHashedId = "CCCC";

            var draftApprenticeshipId = DraftApprenticeshipId;

            RouteData.Values[RouteValueKeys.DraftApprenticeshipHashedId] = DraftApprenticeshipHashedId;

            RoutingFeature.Setup(f => f.RouteData).Returns(RouteData);
            EncodingService.Setup(h => h.TryDecode(DraftApprenticeshipHashedId, EncodingType.ApprenticeshipId, out draftApprenticeshipId)).Returns(false);

            return this;
        }

        public AuthorizationContextProviderTestsFixture SetUserRef(Guid? userRef)
        {
            UserRef = userRef;
            UserRefClaimValue = UserRef == null ? null : UserRef.ToString();

            var userIdClaimValue = UserRefClaimValue;

            UserService.Setup(a => a.IsUserAuthenticated()).Returns(true);
            UserService.Setup(a => a.TryGetUserClaimValue(EmployeeClaims.Id, out userIdClaimValue)).Returns(true);

            return this;
        }

        public AuthorizationContextProviderTestsFixture SetInvalidUserRef()
        {
            UserRefClaimValue = "XXXXX";

            var userIdClaimValue = UserRefClaimValue;

            UserService.Setup(a => a.IsUserAuthenticated()).Returns(true);
            UserService.Setup(a => a.TryGetUserClaimValue(EmployeeClaims.Id, out userIdClaimValue)).Returns(true);

            return this;
        }

        public AuthorizationContextProviderTestsFixture SetValidApprenticeshipId()
        {
            ApprenticeshipHashedId = "XYRZ";
            ApprenticeshipId = 887;

            long decodedApprenticeshipId;
            RouteData.Values[RouteValueKeys.ApprenticeshipHashedId] = ApprenticeshipHashedId;

            RoutingFeature.Setup(f => f.RouteData).Returns(RouteData);
            EncodingService.Setup(h => h.TryDecode(ApprenticeshipHashedId, EncodingType.ApprenticeshipId, out decodedApprenticeshipId)).Returns(true);

            return this;
        }
    }
}
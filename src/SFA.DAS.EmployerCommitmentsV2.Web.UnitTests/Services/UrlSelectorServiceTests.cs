using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class UrlSelectorServiceTests
    {
        [Test]
        public void Constructor_ValidArguments_ShouldNotThrowException()
        {
            var fixtures = new UrlSelectorServiceTestFixtures();

            fixtures.CreateSut();

            Assert.Pass("Did not get an exception");
        }

        [Test]
        public void Constructor_NullAuthorizationServiceArguments_ShouldThrowArgumentNullException()
        {
            var fixtures = new UrlSelectorServiceTestFixtures();

            Assert.Throws<ArgumentNullException>(() => fixtures.CreateSut(authorizationService: null));
        }

        [Test]
        public void Constructor_NullLinkGeneratorArguments_ShouldThrowArgumentNullException()
        {
            var fixtures = new UrlSelectorServiceTestFixtures();

            Assert.Throws<ArgumentNullException>(() => fixtures.CreateSut(linkGenerator: null));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void RedirectToCohortDetails_WithEnhancedApprovalsToggleSet_ShouldReturnAppropriateLink(bool enableEnhancedApprovals)
        {
            var fixtures = new UrlSelectorServiceTestFixtures()
                                .SetEnhancedApproval(enableEnhancedApprovals)
                                .SetCohortLinks();

            var sut = fixtures.CreateSut();

            var action = sut.RedirectToCohortDetails(Mock.Of<IUrlHelper>(), "AC1234", "CO1234");

            if (enableEnhancedApprovals)
            {
                Assert.IsInstanceOf<RedirectToActionResult>(action);
            }
            else
            {
                Assert.IsInstanceOf<RedirectResult>(action);
            }
        }

        [TestCase(Party.Provider, true, true)]
        [TestCase(Party.Provider, false, true)]
        [TestCase(Party.Employer, true, false)]
        [TestCase(Party.Employer, false, false)]
        public void RedirectToV1IfCohortWithOtherParty_WithEnhancedApprovalsToggleSet_ShouldReturnAppropriateLink(Party party, bool enableEnhancedApprovals, bool expectRedirectToV1)
        {
            var fixtures = new UrlSelectorServiceTestFixtures()
                .SetEnhancedApproval(enableEnhancedApprovals)
                .SetCommitmentParty(party)
                .SetCohortLinks();

            var sut = fixtures.CreateSut();
            var cohort = new GetCohortResponse {WithParty = party};

            var action = sut.RedirectToV1IfCohortWithOtherParty("AC1234", "CO1234", cohort);

            if (expectRedirectToV1)
            {
                Assert.IsInstanceOf<RedirectResult>(action);
            }
            else
            {
                Assert.IsNull(action);
            }
        }
    }

    public class UrlSelectorServiceTestFixtures
    {
        public UrlSelectorServiceTestFixtures()
        {
            AuthorizationServiceMock = new Mock<IAuthorizationService>();                
            LinkGeneratorMock = new Mock<ILinkGenerator>();
            EncodingServiceMock = new Mock<IEncodingService>();
            CommitmentsApiClientMock = new Mock<ICommitmentsApiClient>();
        }

        public Mock<IAuthorizationService> AuthorizationServiceMock { get; }
        public IAuthorizationService AuthorizationService => AuthorizationServiceMock.Object;

        public Mock<ILinkGenerator> LinkGeneratorMock { get; }
        public ILinkGenerator LinkGenerator => LinkGeneratorMock.Object;

        public Mock<IEncodingService> EncodingServiceMock { get; }
        public IEncodingService EncodingService => EncodingServiceMock.Object;

        public Mock<ICommitmentsApiClient> CommitmentsApiClientMock { get; }
        public ICommitmentsApiClient CommitmentsApiClient => CommitmentsApiClientMock.Object;


        public UrlSelectorServiceTestFixtures SetEnhancedApprovalOn()
        {
            return SetEnhancedApproval(true);
        }

        public UrlSelectorServiceTestFixtures SetEnhancedApproval(bool enabled )
        {
            AuthorizationServiceMock
                .Setup(au => au.IsAuthorized(It.IsAny<string[]>()))
                .Returns(enabled);

            return this;
        }

        public UrlSelectorServiceTestFixtures SetCohortLinks()
        {
            LinkGeneratorMock
                .Setup(lg => lg.CommitmentsLink(It.IsAny<string>()))
                .Returns("V1");

            return this;
        }

        public UrlSelectorServiceTestFixtures SetCommitmentParty(Party party)
        {
            CommitmentsApiClientMock
                .Setup(ca => ca.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetCohortResponse {WithParty = party});

            return this;
        }

        public UrlSelectorService CreateSut()
        {
            return new UrlSelectorService(
                AuthorizationService,
                LinkGenerator
                );
        }

        public UrlSelectorService CreateSut(
            IAuthorizationService authorizationService,
            ILinkGenerator linkGenerator = null,
            IEncodingService encodingService = null,
            ICommitmentsApiClient commitmentsApiClient = null
        )
        {
            return new UrlSelectorService(
                authorizationService,
                LinkGenerator
            );
        }

        public UrlSelectorService CreateSut(ILinkGenerator linkGenerator)
        {
            return new UrlSelectorService(
                AuthorizationService,
                linkGenerator
            );
        }
    }
}

using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Enums;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Extensions
{
    [TestFixture]
    [Parallelizable]
    public class ILinkGeneratorExtensionsTests
    {
        private ILinkGeneratorExtensionsTestsFixture _fixture;
        [SetUp]
        public void Arrange()
        {
            _fixture = new ILinkGeneratorExtensionsTestsFixture();
        }

        [Test, AutoData]
        public void CohortDetails_BuildsPathCorrectly(string accountHashedId, string cohortReference)
        {
            var url = _fixture.Sut.CohortDetails(accountHashedId, cohortReference);

            Assert.AreEqual($"{_fixture.CommitmentsLink}accounts/{accountHashedId}/apprentices/{cohortReference}/details", url);
        }

        [Test, AutoData]
        public void YourOrganisationsAndAgreements_BuildsPathCorrectly(string accountHashedId)
        {
            var url = _fixture.Sut.YourOrganisationsAndAgreements(accountHashedId);

            Assert.AreEqual($"{_fixture.AccountsLink}accounts/{accountHashedId}/agreements", url);
        }

        [Test, AutoData]
        public void PayeSchemes_BuildsPathCorrectly(string accountHashedId)
        {
            var url = _fixture.Sut.PayeSchemes(accountHashedId);

            Assert.AreEqual($"{_fixture.AccountsLink}accounts/{accountHashedId}/schemes", url);
        }

        [Test, AutoData]
        public void ViewApprentice_BuildsPathCorrectly(string accountHashedId, string cohortReference, string draftApprenticeshipHashedId)
        {
            var url = _fixture.Sut.ViewApprentice(accountHashedId, cohortReference, draftApprenticeshipHashedId);

            Assert.AreEqual($"{_fixture.CommitmentsLink}accounts/{accountHashedId}/apprentices/{cohortReference}/apprenticeships/{draftApprenticeshipHashedId}/view", url);
        }

        [Test, AutoData]
        public void DeleteApprentice_BuildsPathCorrectly_WhenEnhancedApproval_IsTrue(string accountHashedId, string cohortReference, string draftApprenticeshipHashedId, Origin origin)
        {
            _fixture.SetUpForEnhancedApproval(true);
            var url = _fixture.Sut.DeleteApprentice(_fixture.AuthorizationService.Object, accountHashedId, cohortReference, draftApprenticeshipHashedId, origin);

            Assert.AreEqual($"{_fixture.CommitmentsV2Link}{accountHashedId}/unapproved/{cohortReference}/apprentices/{draftApprenticeshipHashedId}/Delete/{origin}", url);
        }

        [Test, AutoData]
        public void DeleteApprentice_BuildsPathCorrectly_WhenEnhancedApproval_IsFalse(string accountHashedId, string cohortReference, string draftApprenticeshipHashedId, Origin origin)
        {
            _fixture.SetUpForEnhancedApproval(false);
            var url = _fixture.Sut.DeleteApprentice(_fixture.AuthorizationService.Object, accountHashedId, cohortReference, draftApprenticeshipHashedId, origin);

            Assert.AreEqual($"{_fixture.CommitmentsLink}accounts/{accountHashedId}/apprentices/{cohortReference}/apprenticeships/{draftApprenticeshipHashedId}/delete", url);
        }
    }

    public class ILinkGeneratorExtensionsTestsFixture
    {
        public Mock<ILinkGenerator> MockILinkGenerator;
        public ILinkGenerator Sut => MockILinkGenerator.Object;
        public string AccountsLink => "https://accounts.com/";
        public string CommitmentsLink => "https://commitments.com/";
        public string CommitmentsV2Link => "https://commitmentsV2.com/";

        public Mock<IAuthorizationService> AuthorizationService;

        public ILinkGeneratorExtensionsTestsFixture()
        {
            MockILinkGenerator = new Mock<ILinkGenerator>();
            MockILinkGenerator.Setup(x => x.AccountsLink(It.IsAny<string>())).Returns((string path) => AccountsLink + path);
            MockILinkGenerator.Setup(x => x.CommitmentsLink(It.IsAny<string>())).Returns((string path) => CommitmentsLink + path);
            MockILinkGenerator.Setup(x => x.CommitmentsV2Link(It.IsAny<string>())).Returns((string path) => CommitmentsV2Link + path);

            AuthorizationService = new Mock<IAuthorizationService>();
        }

        public ILinkGeneratorExtensionsTestsFixture SetUpForEnhancedApproval(bool enhancedApproval)
        {
            AuthorizationService.Setup(y => y.IsAuthorized(EmployerFeature.EnhancedApproval)).Returns(enhancedApproval);

            return this;
        }
    }
}
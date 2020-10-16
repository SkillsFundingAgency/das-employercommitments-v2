using Moq;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class ApprenticeControllerTestBase
    {
        protected const string _apprenticeshipDetailsUrl = "https://commitments.apprenticeships.gov.uk/accounts/apprentices/ABC123/details";
        protected const string _apprenticeshipStopUrl = "https://commitments.apprenticeships.gov.uk/accounts/apprentices/ABC123/stop/whentoapply";

        protected Mock<IModelMapper> _mockModelMapper;
        protected Mock<ICookieStorageService<IndexRequest>> _mockCookieStorageService;
        protected Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
        protected Mock<ILinkGenerator> _mockLinkGenerator;

        protected ApprenticeController _controller;
    }
}

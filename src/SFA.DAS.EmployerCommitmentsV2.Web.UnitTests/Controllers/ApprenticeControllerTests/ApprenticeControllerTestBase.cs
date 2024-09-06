using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class ApprenticeControllerTestBase
{
    protected const string ApprenticeshipDetailsUrl = "https://commitments.apprenticeships.gov.uk/accounts/apprentices/ABC123/details";
    protected const string ApprenticeshipStopUrl = "https://commitments.apprenticeships.gov.uk/accounts/apprentices/ABC123/stop/whentoapply";

    protected Mock<IModelMapper> MockModelMapper;
    protected Mock<Interfaces.ICookieStorageService<IndexRequest>> MockCookieStorageService;
    protected Mock<ICommitmentsApiClient> MockCommitmentsApiClient;
    protected Mock<ICacheStorageService> CacheStorageService;

    protected ApprenticeController Controller;
}
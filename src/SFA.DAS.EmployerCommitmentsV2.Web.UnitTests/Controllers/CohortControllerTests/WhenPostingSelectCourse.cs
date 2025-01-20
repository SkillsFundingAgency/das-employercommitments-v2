using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenPostingSelectCourse
{
    [Test, MoqAutoData]
    public async Task ThenReturnsRedirect(
        AddApprenticeshipCacheModel cacheModel,
        SelectCourseViewModel viewModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        cacheStorageService
           .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(viewModel.ApprenticeshipSessionKey.Value))
           .ReturnsAsync(cacheModel);

        var result = await controller.SelectCourse(viewModel) as RedirectToActionResult;
        result.Should().NotBeNull();
        result.ActionName.Should().BeEquivalentTo("SelectDeliveryModel");
        result.RouteValues["AccountHashedId"].Should().Be(cacheModel.AccountHashedId);
        result.RouteValues["ApprenticeshipSessionKey"].Should().Be(cacheModel.ApprenticeshipSessionKey);
    }
}
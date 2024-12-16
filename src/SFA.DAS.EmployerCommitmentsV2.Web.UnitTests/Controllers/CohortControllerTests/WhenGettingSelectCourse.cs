using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenGettingSelectCourse
{
    [Test, MoqAutoData]
    public async Task ThenReturnsView(
        AddApprenticeshipCacheModel cacheModel,
        SelectCourseViewModel viewModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IModelMapper> modelMapper,
        [Greedy] CohortController controller)
    {
        cacheStorageService
           .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
           .ReturnsAsync(cacheModel);

        modelMapper.Setup(x => x.Map<SelectCourseViewModel>(
            It.Is<AddApprenticeshipCacheModel>(r => r == cacheModel)))
            .ReturnsAsync(viewModel);

        var result = await controller.SelectCourse(cacheModel.ApprenticeshipSessionKey) as ViewResult;

        result.Should().NotBeNull();
        result.ViewName.Should().BeSameAs("SelectCourse");
        result.Model.Should().BeSameAs(viewModel);

    }
}
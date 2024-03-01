using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Cookies;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

[TestFixture]
public class ManageApprenticesTests
{
    [Test, MoqAutoData]
    public async Task ThenTheMappedViewModelIsReturned(
        IndexRequest request,
        IndexViewModel expectedViewModel,
        [Frozen] Mock<IModelMapper> apprenticeshipMapper,
        [Greedy] ApprenticeController controller)
    {
        //Arrange
        apprenticeshipMapper
            .Setup(mapper => mapper.Map<IndexViewModel>(request))
            .ReturnsAsync(expectedViewModel);

        //Act
        var result = await controller.Index(request) as ViewResult;
        var actualModel = result.Model as IndexViewModel;

        //Assert
        actualModel.Should().BeEquivalentTo(expectedViewModel);
    }

    [Test, MoqAutoData]
    public async Task Then_SortedByHeaderClassName_Set(
        IndexRequest request,
        IndexViewModel expectedViewModel,
        [Frozen] Mock<IModelMapper> apprenticeshipMapper,
        [Greedy] ApprenticeController controller)
    {
        //Arrange
        expectedViewModel.FilterModel.ReverseSort = false;
        apprenticeshipMapper
            .Setup(mapper => mapper.Map<IndexViewModel>(request))
            .ReturnsAsync(expectedViewModel);

        //Act
        var result = await controller.Index(request) as ViewResult;
        var actualModel = result.Model as IndexViewModel;

        //Assert
        actualModel.SortedByHeaderClassName.Should().EndWith("das-table__sort--asc");
    }

    [Test, MoqAutoData]
    public async Task Then_Will_Update_Search_Details_To_Cookies(
        IndexRequest request,
        IndexViewModel expectedViewModel,
        [Frozen] Mock<IModelMapper> apprenticeshipMapper,
        [Frozen] Mock<Interfaces.ICookieStorageService<IndexRequest>> cookieService,
        [Greedy] ApprenticeController controller)
    {
        //Arrange
        request.FromSearch = false;

        apprenticeshipMapper
            .Setup(mapper => mapper.Map<IndexViewModel>(It.IsAny<IndexRequest>()))
            .ReturnsAsync(expectedViewModel);

        //Act
        await controller.Index(request);

        //Assert
        cookieService.Verify(x => x.Update(CookieNames.ManageApprentices, request, It.IsAny<int>()));
        cookieService.Verify(x => x.Get(It.IsAny<string>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_Will_Get_Saved_Search_Details_From_Cookies(
        IndexRequest request,
        IndexRequest savedRequest,
        IndexViewModel expectedViewModel,
        [Frozen] Mock<IModelMapper> apprenticeshipMapper,
        [Frozen] Mock<Interfaces.ICookieStorageService<IndexRequest>> cookieService,
        [Greedy] ApprenticeController controller)
    {
        //Arrange
        request.FromSearch = true;

        apprenticeshipMapper
            .Setup(mapper => mapper.Map<IndexViewModel>(It.IsAny<IndexRequest>()))
            .ReturnsAsync(expectedViewModel);

        cookieService
            .Setup(x => x.Get(CookieNames.ManageApprentices))
            .Returns(savedRequest);

        //Act
        await controller.Index(request);

        //Assert
        cookieService.Verify(x => x.Get(CookieNames.ManageApprentices));
    }

    [Test, MoqAutoData]
    public async Task Then_Will_Used_Saved_Search_Details_From_Cookies(
        IndexRequest request,
        IndexRequest savedRequest,
        IndexViewModel expectedViewModel,
        [Frozen] Mock<IModelMapper> apprenticeshipMapper,
        [Frozen] Mock<Interfaces.ICookieStorageService<IndexRequest>> cookieService,
        [Greedy] ApprenticeController controller)
    {
        //Arrange
        request.FromSearch = true;

        apprenticeshipMapper
            .Setup(mapper => mapper.Map<IndexViewModel>(It.IsAny<IndexRequest>()))
            .ReturnsAsync(expectedViewModel);

        cookieService
            .Setup(x => x.Get(CookieNames.ManageApprentices))
            .Returns(savedRequest);

        //Act
        await controller.Index(request);

        //Assert
        apprenticeshipMapper.Verify(mapper => mapper.Map<IndexViewModel>(savedRequest));
    }

    [Test, MoqAutoData]
    public async Task Then_Will_Update_Saved_Search_Details_If_From_Cookies(
        IndexRequest request,
        IndexRequest savedRequest,
        IndexViewModel expectedViewModel,
        [Frozen] Mock<IModelMapper> apprenticeshipMapper,
        [Frozen] Mock<Interfaces.ICookieStorageService<IndexRequest>> cookieService,
        [Greedy] ApprenticeController controller)
    {
        //Arrange
        request.FromSearch = true;

        apprenticeshipMapper
            .Setup(mapper => mapper.Map<IndexViewModel>(It.IsAny<IndexRequest>()))
            .ReturnsAsync(expectedViewModel);

        cookieService
            .Setup(x => x.Get(CookieNames.ManageApprentices))
            .Returns(savedRequest);

        //Act
        await controller.Index(request);

        //Assert
        cookieService.Verify(x => x.Update(
            CookieNames.ManageApprentices, It.IsAny<IndexRequest>(), It.IsAny<int>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_Will_Used_Current_Search_Details_If_Saved_Details_Not_Available(
        IndexRequest request,
        IndexViewModel expectedViewModel,
        [Frozen] Mock<IModelMapper> apprenticeshipMapper,
        [Frozen] Mock<Interfaces.ICookieStorageService<IndexRequest>> cookieService,
        [Greedy] ApprenticeController controller)
    {
        //Arrange
        request.FromSearch = true;

        apprenticeshipMapper
            .Setup(mapper => mapper.Map<IndexViewModel>(It.IsAny<IndexRequest>()))
            .ReturnsAsync(expectedViewModel);

        cookieService
            .Setup(x => x.Get(CookieNames.ManageApprentices))
            .Returns((IndexRequest)null);

        //Act
        await controller.Index(request);

        //Assert
        apprenticeshipMapper.Verify(mapper => mapper.Map<IndexViewModel>(request));

        cookieService.Verify(x => x.Update(
            CookieNames.ManageApprentices, It.IsAny<IndexRequest>(), It.IsAny<int>()), Times.Once);

    }
}
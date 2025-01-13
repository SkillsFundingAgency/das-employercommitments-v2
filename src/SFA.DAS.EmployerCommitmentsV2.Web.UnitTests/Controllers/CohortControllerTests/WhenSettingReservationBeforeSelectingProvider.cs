using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenSettingReservationBeforeSelectingProvider
{
    [Test, MoqAutoData]
    public async Task Then_Redirect_To_Select_Provider(
        Guid? reservationId, 
        string courseCode, 
        string startMonthYear, 
        Guid? apprenticeshipSessionKey,
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        cacheModel.ApprenticeshipSessionKey = apprenticeshipSessionKey.Value;
        cacheStorageService
            .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(apprenticeshipSessionKey.Value))
            .ReturnsAsync(cacheModel);

        var result = await controller.SetReservation(reservationId, courseCode, startMonthYear, apprenticeshipSessionKey) as RedirectToActionResult;

        result.Should().NotBeNull();
        result.ActionName.Should().Be("SelectProvider");
        result.RouteValues["AccountHashedId"].Should().Be(cacheModel.AccountHashedId);
        result.RouteValues["ApprenticeshipSessionKey"].Should().Be(cacheModel.ApprenticeshipSessionKey);
    }

    [Test, MoqAutoData]
    public async Task Then_Reservation_Data_Is_Saved_In_Cache(
        Guid? reservationId,
        string courseCode,
        string startMonthYear,
        Guid? apprenticeshipSessionKey,
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        cacheModel.ApprenticeshipSessionKey = apprenticeshipSessionKey.Value;
        cacheStorageService
            .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(apprenticeshipSessionKey.Value))
            .ReturnsAsync(cacheModel);

        var result = await controller.SetReservation(reservationId, courseCode, startMonthYear, apprenticeshipSessionKey) as RedirectToActionResult;

        cacheStorageService.Verify(x => x.SaveToCache(apprenticeshipSessionKey.Value,
            It.Is<AddApprenticeshipCacheModel>(p =>
                p.ReservationId == reservationId && p.CourseCode == courseCode && p.StartMonthYear == startMonthYear),
            It.IsAny<int>()));

        result.Should().NotBeNull();
        result.ActionName.Should().Be("SelectProvider");
        result.RouteValues["AccountHashedId"].Should().Be(cacheModel.AccountHashedId);
        result.RouteValues["ApprenticeshipSessionKey"].Should().Be(cacheModel.ApprenticeshipSessionKey);
    }
}
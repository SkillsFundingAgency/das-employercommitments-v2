using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models.ApprenticeFilterModelTests;

public class WhenGettingRouteData
{
    [Test, AutoData]
    public void Then_Contains_Item_For_Each_Search_And_Filter_Value(
        ApprenticesFilterModel filterModel)
    {
        filterModel.RouteData.Should().BeEquivalentTo(new Dictionary<string, string>
        {
            {nameof(filterModel.SearchTerm), filterModel.SearchTerm },
            {nameof(filterModel.SelectedProvider), filterModel.SelectedProvider},
            {nameof(filterModel.SelectedCourse), filterModel.SelectedCourse},
            {nameof(filterModel.SelectedStatus), filterModel.SelectedStatus.ToString()},
            {nameof(filterModel.SelectedAlert), filterModel.SelectedAlert.ToString()},
            {nameof(filterModel.SelectedEndDate), filterModel.SelectedEndDate.Value.ToString("yyyy-MM-dd")},
            {nameof(filterModel.SelectedApprenticeConfirmation), filterModel.SelectedApprenticeConfirmation.ToString()}
        });
    }

    [Test, AutoData]
    public void Then_Not_Contain_Item_For_PageNumber(
        ApprenticesFilterModel filterModel)
    {
        filterModel.RouteData.Should().NotContain(new KeyValuePair<string, string>(
            nameof(filterModel.PageNumber), filterModel.PageNumber.ToString() )
        );
    }
}
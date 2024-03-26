using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models.ApprenticeFilterModelTests;

public class WhenGettingPagedRecordsFrom
{
    [Test, AutoData]
    public void And_PageNumber_1_Then_Should_Be_1(ApprenticesFilterModel filterModel)
    {
        filterModel.PageNumber = 1;

        filterModel.PagedRecordsFrom.Should().Be(1);
    }

    [Test, AutoData]
    public void And_PageNumber_2_Then_Should_Be_PageSize_Plus_1(ApprenticesFilterModel filterModel)
    {
        filterModel.PageNumber = 2;

        filterModel.PagedRecordsFrom.Should().Be(Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage+1);
    }

    [Test, AutoData]
    public void And_PageNumber_3_Then_Should_Be_Double_PageSize_Plus_1(ApprenticesFilterModel filterModel)
    {
        filterModel.PageNumber = 3;

        filterModel.PagedRecordsFrom.Should().Be(2 * Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage+1);
    }

    [Test, AutoData]
    public void And_PageNumber_1_And_0_Records_Found_Then_Should_Be_0(ApprenticesFilterModel filterModel)
    {
        filterModel.PageNumber = 1;
        filterModel.TotalNumberOfApprenticeshipsFound = 0;

        filterModel.PagedRecordsFrom.Should().Be(0);
    }
}
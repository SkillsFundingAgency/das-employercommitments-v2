using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models.ApprenticeFilterModelTests;

public class WhenGettingPagedRecordsTo
{
    [Test, AutoData]
    public void And_PageNumber_1_Then_Should_Be_PageSize(ApprenticesFilterModel filterModel)
    {
        filterModel.PageNumber = 1;
        filterModel.TotalNumberOfApprenticeshipsFound = 20 * Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage;

        filterModel.PagedRecordsTo.Should().Be(Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage);
    }

    [Test, AutoData]
    public void And_PageNumber_2_Then_Should_Be_Double_PageSize(ApprenticesFilterModel filterModel)
    {
        filterModel.PageNumber = 2;
        filterModel.TotalNumberOfApprenticeshipsFound = 20 * Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage;

        filterModel.PagedRecordsTo.Should().Be(2 * Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage);
    }

    [Test, AutoData]
    public void And_PageNumber_3_Then_Should_Be_Triple_PageSize(ApprenticesFilterModel filterModel)
    {
        filterModel.PageNumber = 3;
        filterModel.TotalNumberOfApprenticeshipsFound = 20 * Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage;

        filterModel.PagedRecordsTo.Should().Be(3 * Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage);
    }

    [Test, AutoData]
    public void And_TotalRecords_Less_Than_Calculated_PagedRecordsTo_Then_Is_TotalRecords(ApprenticesFilterModel filterModel)
    {
        filterModel.PageNumber = 3;
        filterModel.TotalNumberOfApprenticeshipsFound = 3 * Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage - 20;

        filterModel.PagedRecordsTo.Should().Be(filterModel.TotalNumberOfApprenticeshipsFound);
    }
}
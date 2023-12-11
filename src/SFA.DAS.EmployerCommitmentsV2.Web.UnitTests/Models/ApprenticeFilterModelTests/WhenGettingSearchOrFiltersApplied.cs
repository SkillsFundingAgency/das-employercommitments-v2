using FluentAssertions;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models.ApprenticeFilterModelTests;

public class WhenGettingSearchOrFiltersApplied
{
    [Test]
    public void And_Has_SearchTerm_Then_True()
    {
        var filterModel = new ApprenticesFilterModel
        {
            SearchTerm = "asedfas"
        };

        filterModel.SearchOrFiltersApplied.Should().BeTrue();
    }

    [Test]
    public void And_Has_SelectedEmployer_Then_True()
    {
        var filterModel = new ApprenticesFilterModel
        {
            SelectedProvider = "asedfas"
        };

        filterModel.SearchOrFiltersApplied.Should().BeTrue();
    }

    [Test]
    public void And_Has_SelectedCourse_Then_True()
    {
        var filterModel = new ApprenticesFilterModel
        {
            SelectedCourse = "asedfas"
        };

        filterModel.SearchOrFiltersApplied.Should().BeTrue();
    }

    [Test]
    public void And_Has_SelectedStatus_Then_True()
    {
        var filterModel = new ApprenticesFilterModel
        {
            SelectedStatus = ApprenticeshipStatus.WaitingToStart
        };

        filterModel.SearchOrFiltersApplied.Should().BeTrue();
    }

    [Test]
    public void And_Has_SelectedEndDate_Then_True()
    {
        var filterModel = new ApprenticesFilterModel
        {
            SelectedEndDate = DateTime.Today
        };

        filterModel.SearchOrFiltersApplied.Should().BeTrue();
    }

    [Test]
    public void And_Has_SelectedAlert_Then_True()
    {
        var filterModel = new ApprenticesFilterModel
        {
            SelectedAlert = Alerts.IlrDataMismatch
        };

        filterModel.SearchOrFiltersApplied.Should().BeTrue();
    }

    [Test]
    public void And_No_Search_Or_Filter_Then_False()
    {
        var filterModel = new ApprenticesFilterModel();

        filterModel.SearchOrFiltersApplied.Should().BeFalse();
    }

       
}
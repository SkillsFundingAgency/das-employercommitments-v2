using System;
using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Extensions;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models.ApprenticeFilterModelTests;

public class WhenGettingFiltersUsedMessage
{
    [Test]
    public void And_No_Search_And_No_Filters_Then_Null()
    {
        var filterModel = new ApprenticesFilterModel();

        filterModel.FiltersUsedMessage.Should().Be(HtmlString.Empty);
    }

    [Test, AutoData]
    public void And_Search_And_No_Filters_Then_Quoted_SearchTerm(
        string searchTerm)
    {
        var filterModel = new ApprenticesFilterModel
        {
            SearchTerm = searchTerm
        };

        filterModel.FiltersUsedMessage.Value.Should().Be($"matching <strong>‘{searchTerm}’</strong>");
    }

    [Test]
    public void And_Search_And_Filters_Has_Html_Then_Html_Encodes_It()
    {
        var htmlSearchTerm = "<script>alert('hi');</script>";
        var filterModel = new ApprenticesFilterModel
        {
            SearchTerm = htmlSearchTerm,
            SelectedCourse = htmlSearchTerm,
            SelectedProvider = htmlSearchTerm
        };

        filterModel.FiltersUsedMessage.Value.Should().Be($"matching <strong>‘{WebUtility.HtmlEncode(htmlSearchTerm)}’</strong>, <strong>{WebUtility.HtmlEncode(htmlSearchTerm)}</strong> and <strong>{WebUtility.HtmlEncode(htmlSearchTerm)}</strong>");
    }

    [Test, AutoData]
    public void And_No_Search_And_SelectedEmployer_Then_SelectedEmployer(
        string selectedEmployer)
    {
        var filterModel = new ApprenticesFilterModel
        {
            SelectedProvider = selectedEmployer
        };

        filterModel.FiltersUsedMessage.Value.Should().Be($"matching <strong>{selectedEmployer}</strong>");
    }

    [Test, AutoData]
    public void And_No_Search_And_SelectedCourse_Then_SelectedCourse(
        string selectedCourse)
    {
        var filterModel = new ApprenticesFilterModel
        {
            SelectedCourse = selectedCourse
        };

        filterModel.FiltersUsedMessage.Value.Should().Be($"matching <strong>{selectedCourse}</strong>");
    }

    [Test, AutoData]
    public void And_No_Search_And_SelectedStatus_Then_SelectedStatus(
        ApprenticeshipStatus selectedStatus)
    {
        var filterModel = new ApprenticesFilterModel
        {
            SelectedStatus = selectedStatus
        };

        filterModel.FiltersUsedMessage.Value.Should().Be($"matching <strong>{selectedStatus.GetDescription()}</strong>");
    }

    [Test, AutoData]
    public void And_No_Search_And_SelectedEndDate_Then_SelectedEndDate(
        DateTime selectedEndDate)
    {
        var filterModel = new ApprenticesFilterModel
        {
            SelectedEndDate = selectedEndDate
        };

        filterModel.FiltersUsedMessage.Value.Should().Be($"matching <strong>{selectedEndDate.ToGdsFormatWithoutDay()}</strong>");
    }


    [Test, AutoData]
    public void And_No_Search_And_SelectedStatus_Then_SelectedStatus(Alerts selectedAlert)
    {
        var filterModel = new ApprenticesFilterModel
        {
            SelectedAlert = selectedAlert
        };

        filterModel.FiltersUsedMessage.Value.Should().Be($"matching <strong>{selectedAlert.GetDescription()}</strong>");
    }


    [Test, AutoData]
    public void And_Search_And_SelectedProvider_Then_SearchTerm_And_SelectedProvider(
        string searchTerm,
        string selectedProvider)
    {
        var filterModel = new ApprenticesFilterModel
        {
            SearchTerm = searchTerm,
            SelectedProvider = selectedProvider
        };

        filterModel.FiltersUsedMessage.Value
            .Should().Be($"matching <strong>‘{searchTerm}’</strong>" +
                         $" and <strong>{selectedProvider}</strong>");
    }

    [Test, AutoData]
    public void And_Search_And_SelectedProvider_And_SelectedCourse_Then_SearchTerm_Comma_SelectedProvider_And_SelectedCourse(
        string searchTerm,
        string selectedProvider,
        string selectedCourse)
    {
        var filterModel = new ApprenticesFilterModel
        {
            SearchTerm = searchTerm,
            SelectedProvider = selectedProvider,
            SelectedCourse = selectedCourse
        };

        filterModel.FiltersUsedMessage.Value
            .Should().Be($"matching <strong>‘{searchTerm}’</strong>" +
                         $", <strong>{selectedProvider}</strong>" +
                         $" and <strong>{selectedCourse}</strong>");
    }

    [Test, AutoData]
    public void And_SelectedEmployer_And_SelectedCourse_And_SelectedEndDate_Then_SelectedEmployer_Comma_SelectedCourse_And_SelectedEndDate(
        string selectedEmployer,
        string selectedCourse,
        DateTime selectedEndDate)
    {
        var filterModel = new ApprenticesFilterModel
        {
            SelectedProvider = selectedEmployer,
            SelectedCourse = selectedCourse,
            SelectedEndDate = selectedEndDate
        };

        filterModel.FiltersUsedMessage.Value
            .Should().Be($"matching <strong>{selectedEmployer}</strong>" +
                         $", <strong>{selectedCourse}</strong>" +
                         $" and <strong>{selectedEndDate.ToGdsFormatWithoutDay()}</strong>");
    }
}
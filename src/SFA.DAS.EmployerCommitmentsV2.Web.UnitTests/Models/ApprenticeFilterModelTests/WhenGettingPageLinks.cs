using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models.ApprenticeFilterModelTests
{
    public class WhenGettingPageLinks
    {
        [Test]
        public void Then_Adds_PageLink_For_Every_Page()
        {
            var filterModel = new ApprenticesFilterModel
            {
                SearchTerm = "asedfas",
                SelectedProvider = "asdsad",
                SelectedCourse = "iknjso",
                SelectedStatus = ApprenticeshipStatus.WaitingToStart,
                SelectedAlert  = Alerts.ChangesForReview,
                SelectedEndDate = DateTime.Today,
                TotalNumberOfApprenticeshipsFound = Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage * 3,
                SortField = "gsd",
                ReverseSort = false
            };

            var pageLinks = filterModel.PageLinks.Where(link =>
                link.Label.ToUpper() != "PREVIOUS"
                && link.Label.ToUpper() != "NEXT").ToList();

            for (var i = 0; i < 3; i++)
            {
                pageLinks[i].Label.Should().Be($"{i + 1}");
                pageLinks[i].AriaLabel.Should().Be($"Page {i + 1}");
                pageLinks[i].RouteData.Should().BeEquivalentTo(new Dictionary<string, string>
                    {
                        {nameof(filterModel.SearchTerm), filterModel.SearchTerm },
                        {nameof(filterModel.SelectedProvider), filterModel.SelectedProvider},
                        {nameof(filterModel.SelectedCourse), filterModel.SelectedCourse},
                        {nameof(filterModel.SelectedStatus), filterModel.SelectedStatus.ToString()},
                        {nameof(filterModel.SelectedAlert), filterModel.SelectedAlert.ToString()},
                        {nameof(filterModel.SelectedEndDate), filterModel.SelectedEndDate.Value.ToString("yyyy-MM-dd")},
                        {nameof(filterModel.SortField), filterModel.SortField},
                        {nameof(filterModel.ReverseSort), filterModel.ReverseSort.ToString()},
                        {nameof(filterModel.PageNumber), (i+1).ToString() }
                    });
            }
        }

        [Test]
        public void And_PageNumber_1_Then_Sets_Current_On_1()
        {
            var filterModel = new ApprenticesFilterModel
            {
                PageNumber = 1,
                TotalNumberOfApprenticeshipsFound = Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage * 3
            };

            var pageLinks = filterModel.PageLinks.Where(link => 
                link.Label.ToUpper() != "PREVIOUS" 
                && link.Label.ToUpper() != "NEXT").ToList();

            pageLinks[0].IsCurrent.Should().BeTrue();
            pageLinks[1].IsCurrent.Should().BeNull();
            pageLinks[2].IsCurrent.Should().BeNull();
        }

        [Test]
        public void And_PageNumber_3_Then_Sets_Current_On_3()
        {
            var filterModel = new ApprenticesFilterModel
            {
                PageNumber = 3,
                TotalNumberOfApprenticeshipsFound = Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage * 3
            };

            var pageLinks = filterModel.PageLinks.Where(link => 
                link.Label.ToUpper() != "PREVIOUS" 
                && link.Label.ToUpper() != "NEXT").ToList();

            pageLinks[0].IsCurrent.Should().BeNull();
            pageLinks[1].IsCurrent.Should().BeNull();
            pageLinks[2].IsCurrent.Should().BeTrue();
        }

        [Test]
        public void And_PageNumber_5_Of_7_Then_Sets_Current_On_5()
        {
            var filterModel = new ApprenticesFilterModel
            {
                PageNumber = 5,
                TotalNumberOfApprenticeshipsFound = Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage * 7
            };

            var pageLinks = filterModel.PageLinks.Where(link => 
                link.Label.ToUpper() != "PREVIOUS" 
                && link.Label.ToUpper() != "NEXT").ToList();

            pageLinks[0].IsCurrent.Should().BeNull();
            pageLinks[1].IsCurrent.Should().BeNull();
            pageLinks[2].IsCurrent.Should().BeTrue();
            pageLinks[3].IsCurrent.Should().BeNull();
            pageLinks[4].IsCurrent.Should().BeNull();
        }

        [Test]
        public void And_3_Pages_Then_Only_3_PageLinks()
        {
            var filterModel = new ApprenticesFilterModel
            {
                PageNumber = 1,
                TotalNumberOfApprenticeshipsFound = Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage * 3
            };

            filterModel.PageLinks.Count(link => 
                link.Label.ToUpper() != "PREVIOUS" 
                && link.Label.ToUpper() != "NEXT")
                .Should().Be(3);
        }

        [Test]
        public void And_More_Than_5_Pages_Then_Only_5_PageLinks()
        {
            var filterModel = new ApprenticesFilterModel
            {
                PageNumber = 1,
                TotalNumberOfApprenticeshipsFound = Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage * 10
            };

            filterModel.PageLinks.Count(link => 
                    link.Label.ToUpper() != "PREVIOUS" 
                    && link.Label.ToUpper() != "NEXT")
                .Should().Be(5);
        }

        [Test]
        public void And_Page_Number_4_Of_5_Then_Links_1_To_5()
        {
            var filterModel = new ApprenticesFilterModel
            {
                PageNumber = 4,
                TotalNumberOfApprenticeshipsFound = Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage * 5
            };

            var pageLinks = filterModel.PageLinks.Where(link =>
                link.Label.ToUpper() != "PREVIOUS"
                && link.Label.ToUpper() != "NEXT").ToList();

            pageLinks[0].Label.Should().Be(1.ToString());
            pageLinks[1].Label.Should().Be(2.ToString());
            pageLinks[2].Label.Should().Be(3.ToString());
            pageLinks[3].Label.Should().Be(4.ToString());
            pageLinks[4].Label.Should().Be(5.ToString());
        }

        [Test]
        public void And_Page_Number_7_Of_10_Then_Links_5_To_9()
        {
            var filterModel = new ApprenticesFilterModel
            {
                PageNumber = 7,
                TotalNumberOfApprenticeshipsFound = Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage * 10
            };

            var pageLinks = filterModel.PageLinks.Where(link =>
                link.Label.ToUpper() != "PREVIOUS"
                && link.Label.ToUpper() != "NEXT").ToList();

            pageLinks[0].Label.Should().Be(5.ToString());
            pageLinks[1].Label.Should().Be(6.ToString());
            pageLinks[2].Label.Should().Be(7.ToString());
            pageLinks[3].Label.Should().Be(8.ToString());
            pageLinks[4].Label.Should().Be(9.ToString());
        }

        [Test]
        public void And_Page_Number_10_Of_10_Then_Links_6_To_10()
        {
            var filterModel = new ApprenticesFilterModel
            {
                PageNumber = 10,
                TotalNumberOfApprenticeshipsFound = Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage * 10
            };

            var pageLinks = filterModel.PageLinks.Where(link =>
                link.Label.ToUpper() != "PREVIOUS"
                && link.Label.ToUpper() != "NEXT").ToList();

            pageLinks[0].Label.Should().Be(6.ToString());
            pageLinks[1].Label.Should().Be(7.ToString());
            pageLinks[2].Label.Should().Be(8.ToString());
            pageLinks[3].Label.Should().Be(9.ToString());
            pageLinks[4].Label.Should().Be(10.ToString());
        }

        [Test]
        public void And_1_Page_Then_No_Next_Or_Previous()
        {
            var filterModel = new ApprenticesFilterModel
            {
                PageNumber = 1,
                TotalNumberOfApprenticeshipsFound = Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage - 1
            };

            var pageLinks = filterModel.PageLinks.ToList();
            
            pageLinks.Any(link => link.Label.ToUpper() == "PREVIOUS").Should().BeFalse();
            pageLinks.Any(link => link.Label.ToUpper() == "NEXT").Should().BeFalse();
        }

        [Test]
        public void And_Not_Last_Page_Then_Adds_Next()
        {
            var filterModel = new ApprenticesFilterModel
            {
                PageNumber = 1,
                TotalNumberOfApprenticeshipsFound = Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage * 6
            };

            var pageLinks = filterModel.PageLinks.ToList();

            pageLinks.Last().Label.Should().Be("Next");
            pageLinks.Last().AriaLabel.Should().Be("Next page");
            pageLinks.Last().RouteData.Should()
                .BeEquivalentTo(pageLinks.Single(link => 
                    link.Label == (filterModel.PageNumber + 1).ToString()).RouteData);
        }

        [Test]
        public void And_Is_Last_Page_Then_No_Next()
        {
            var filterModel = new ApprenticesFilterModel
            {
                PageNumber = 6,
                TotalNumberOfApprenticeshipsFound = Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage * 6
            };

            var pageLinks = filterModel.PageLinks.ToList();

            pageLinks.Last().Label.Should().Be(6.ToString());
        }

        [Test]
        public void And_Not_First_Page_Then_Adds_Previous()
        {
            var filterModel = new ApprenticesFilterModel
            {
                PageNumber = 2,
                TotalNumberOfApprenticeshipsFound = Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage * 6
            };

            var pageLinks = filterModel.PageLinks.ToList();

            pageLinks.First().Label.Should().Be("Previous");
            pageLinks.First().AriaLabel.Should().Be("Previous page");
            pageLinks.First().RouteData.Should()
                .BeEquivalentTo(pageLinks.Single(link => 
                    link.Label == "1").RouteData);
        }

        [Test]
        public void And_Is_First_Page_Then_No_Previous()
        {
            var filterModel = new ApprenticesFilterModel
            {
                PageNumber = 1,
                TotalNumberOfApprenticeshipsFound = Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage * 6
            };

            var pageLinks = filterModel.PageLinks.ToList();

            pageLinks.First().Label.Should().Be(1.ToString());
        }
    }
}
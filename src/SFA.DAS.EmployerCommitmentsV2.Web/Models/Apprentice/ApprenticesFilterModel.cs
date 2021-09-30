using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using System.Linq;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ApprenticesFilterModel
    {
        public int PageNumber { get; set; } = 1;
        public string SearchTerm { get; set; }
        public string SelectedProvider { get; set; }
        public string SelectedCourse { get; set; }
        public ApprenticeshipStatus? SelectedStatus { get; set; }
        public DateTime? SelectedEndDate { get; set; }
        public Alerts? SelectedAlert { get; set; }
        public ConfirmationStatus? SelectedApprenticeConfirmation { get; set; }

        public string SortField { get; set; }
        public bool ReverseSort { get; set; }

        public IEnumerable<string> ProviderFilters { get; set; } = new List<string>();
        public IEnumerable<string> CourseFilters { get; set; } = new List<string>();
        public IEnumerable<ApprenticeshipStatus> StatusFilters { get; set; } = new List<ApprenticeshipStatus>();
        public IEnumerable<DateTime> EndDateFilters { get; set; } = new List<DateTime>();
        public IEnumerable<Alerts> AlertFilters { get; set; } = new List<Alerts>();
        public IEnumerable<ConfirmationStatus> ApprenticeConfirmationFilters =>
                Enum.GetValues(typeof(ConfirmationStatus)).Cast<ConfirmationStatus>().ToList();

        private const int PageSize = Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage;
        public int PagedRecordsFrom => TotalNumberOfApprenticeshipsFound == 0 ? 0 : (PageNumber - 1) * PageSize + 1;
        public int PagedRecordsTo {
            get
            {
                var potentialValue = PageNumber * PageSize;
                return TotalNumberOfApprenticeshipsFound < potentialValue ? TotalNumberOfApprenticeshipsFound: potentialValue;
            }
        }
        public bool ShowSearch => TotalNumberOfApprenticeships >= Constants.ApprenticesSearch.NumberOfApprenticesRequiredForSearch;

        public bool SearchOrFiltersApplied => !string.IsNullOrWhiteSpace(SearchTerm)
                                              || !string.IsNullOrWhiteSpace(SelectedProvider)
                                              || !string.IsNullOrWhiteSpace(SelectedCourse)
                                              || SelectedStatus.HasValue
                                              || SelectedEndDate.HasValue
                                              || SelectedAlert.HasValue
                                              || SelectedApprenticeConfirmation.HasValue;

        public HtmlString FiltersUsedMessage => this.GetFiltersUsedMessage();

        public int TotalNumberOfApprenticeships { get; set; }
        public int TotalNumberOfApprenticeshipsFound { get; set; }
        public int TotalNumberOfApprenticeshipsWithAlertsFound { get; set; }
        
        public IEnumerable<PageLink> PageLinks {
            get
            {
                var links = new List<PageLink>();
                var totalPages = (int)Math.Ceiling((double)TotalNumberOfApprenticeshipsFound / PageSize);
                var totalPageLinks = totalPages < 5 ? totalPages : 5;

                //previous link
                if (totalPages > 1 && PageNumber > 1)
                {
                    links.Add(new PageLink
                    {
                        Label = "Previous",
                        AriaLabel = "Previous page",
                        RouteData = BuildPagedRouteData(PageNumber - 1)
                    });
                }

                //numbered links
                var pageNumberSeed = 1;
                if (totalPages > 5 && PageNumber > 3)
                {
                    pageNumberSeed = PageNumber - 2;

                    if (PageNumber > totalPages - 2)
                        pageNumberSeed = totalPages - 4;
                }

                for (var i = 0; i < totalPageLinks; i++)
                {
                    var link = new PageLink
                    {
                        Label = (pageNumberSeed + i).ToString(),
                        AriaLabel = $"Page {pageNumberSeed + i}",
                        IsCurrent = pageNumberSeed + i == PageNumber? true : (bool?)null,
                        RouteData = BuildPagedRouteData(pageNumberSeed + i)
                    };
                    links.Add(link);
                }

                //next link
                if (totalPages > 1 && PageNumber < totalPages)
                {
                    links.Add(new PageLink
                    {
                        Label = "Next",
                        AriaLabel = "Next page",
                        RouteData = BuildPagedRouteData(PageNumber + 1)
                    });
                }

                return links;
            }
        }

        public Dictionary<string, string> RouteData => BuildRouteData();

        private Dictionary<string, string> BuildRouteData()
        {
            var routeData = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                routeData.Add(nameof(SearchTerm), SearchTerm);
            }

            if (!string.IsNullOrWhiteSpace(SelectedProvider))
            {
                routeData.Add(nameof(SelectedProvider), SelectedProvider);
            }

            if (!string.IsNullOrWhiteSpace(SelectedCourse))
            {
                routeData.Add(nameof(SelectedCourse), SelectedCourse);
            }

            if (SelectedStatus.HasValue)
            {
                routeData.Add(nameof(SelectedStatus), SelectedStatus.Value.ToString());
            }
            
            if (SelectedEndDate.HasValue)
            {
                routeData.Add(nameof(SelectedEndDate), SelectedEndDate.Value.ToString("yyyy-MM-dd"));
            }

            if (SelectedAlert.HasValue)
            {
                routeData.Add(nameof(SelectedAlert), SelectedAlert.Value.ToString());
            }

            if (SelectedApprenticeConfirmation.HasValue)
            {
                routeData.Add(nameof(SelectedApprenticeConfirmation), SelectedApprenticeConfirmation.Value.ToString());
            }

            return routeData;
        }
        
        private Dictionary<string, string> BuildPagedRouteData(int pageNumber)
        {
            var routeData = BuildRouteData();
            
            routeData.Add(nameof(PageNumber), pageNumber.ToString());

            if (!string.IsNullOrEmpty(SortField))
            {
                routeData.Add(nameof(SortField), SortField);

                routeData.Add(nameof(ReverseSort), ReverseSort.ToString());
            }

            return routeData;
        }

        public Dictionary<string, string> BuildSortRouteData(string sortField)
        {
            var routeData = BuildRouteData();

            var reverseSort = !string.IsNullOrEmpty(SortField) 
                              && SortField.ToLower() == sortField.ToLower() 
                              && !ReverseSort;
            routeData.Add(nameof(ReverseSort), reverseSort.ToString());
            routeData.Add(nameof(SortField), sortField);

            return routeData;
        }
        public class PageLink
        {
            public string Label { get; set; }
            public string AriaLabel { get; set; }
            public bool? IsCurrent { get; set; }
            public Dictionary<string, string> RouteData { get; set; }
        }
    }
}
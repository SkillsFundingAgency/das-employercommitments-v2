using System;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class IndexRequest
    {
        private int _pageNumber = 1;
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value > 0 ? value : 1;
        }
        public string AccountHashedId { get; set; }
        public string SortField { get; set; }
        public bool ReverseSort { get; set; }
        public string SearchTerm { get; set; }
        public string SelectedProvider { get; set; }
        public string SelectedCourse { get; set; }
        public DateTime? SelectedEndDate { get; set; }
        public ApprenticeshipStatus? SelectedStatus { get; set; }
        public bool FromSearch { get; set; }
        public Alerts? SelectedAlert { get; set; }
    }
}
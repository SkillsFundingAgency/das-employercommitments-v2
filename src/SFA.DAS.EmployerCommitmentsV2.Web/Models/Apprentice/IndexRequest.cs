using System;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class IndexRequest
    {
        public string HashedAccountId { get; set; }
        public int PageNumber { get; set; }
        public string SortField { get; set; }
        public bool ReverseSort { get; set; }
        public string SearchTerm { get; set; }
        public string SelectedEmployer { get; set; }
        public string SelectedCourse { get; set; }
        public ApprenticeshipStatus? SelectedStatus { get; set; }
        public DateTime? SelectedStartDate { get; set; }
        public DateTime? SelectedEndDate { get; set; }
    }
}
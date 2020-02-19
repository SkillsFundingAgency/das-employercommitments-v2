using System;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class IndexRequest
    {
        public string AccountHashedId { get; set; }
        public int PageNumber { get; set; } = 1;
        public string SortField { get; set; }
        public bool ReverseSort { get; set; }
        public string SearchTerm { get; set; }
        public string SelectedProvider { get; set; }
        public string SelectedCourse { get; set; }
        public DateTime? SelectedEndDate { get; set; }
        public ApprenticeshipStatus? SelectedStatus { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort
{
    public class AssignRequest : IndexRequest
    {
        [Range(1, uint.MaxValue)]
        public long ProviderId { get; set; }
    }
}
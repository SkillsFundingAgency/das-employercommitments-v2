using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort
{
    public class AssignRequest : IndexRequest
    {
        public long ProviderId { get; set; }
    }
}
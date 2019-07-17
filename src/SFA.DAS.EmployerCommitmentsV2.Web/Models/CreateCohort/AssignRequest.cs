using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort
{
    public class AssignRequest : IndexRequest
    {
        [Range(1, uint.MaxValue)]
        public uint UkPrn { get; set; }
    }
}
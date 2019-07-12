using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort
{
    public class AssignRequest : IndexRequest
    {
        public uint UkPrn { get; set; }
    }
}
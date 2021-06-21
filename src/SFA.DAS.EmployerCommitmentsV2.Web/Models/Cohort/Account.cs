using SFA.DAS.CommitmentsV2.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class Account
    {
        public long Id { get; set; }
        public ApprenticeshipEmployerType ApprenticeshipEmployerType { get; set; }
    }
}

using SFA.DAS.EmployerCommitmentsV2.Web.Models.Agency;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Services
{
    public interface IFjaaAgencyService
    {
        Task<bool> AgencyExists(int legalEntityId);
    }
}

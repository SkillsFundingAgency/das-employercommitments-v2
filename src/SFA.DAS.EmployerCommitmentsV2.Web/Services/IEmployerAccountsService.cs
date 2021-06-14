using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Services
{
    public interface IEmployerAccountsService
    {
        Task<Account> GetAccount(long accountId);

        Task<List<LegalEntity>> GetLegalEntitiesForAccount(string accountId);
    }
}

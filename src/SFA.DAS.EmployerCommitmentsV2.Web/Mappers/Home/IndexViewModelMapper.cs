using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Home;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Home
{
    public class IndexViewModelMapper : IMapper<IndexRequest, IndexViewModel>
    {
        public Task<IndexViewModel> Map(IndexRequest source)
        {
            return Task.FromResult(new IndexViewModel
            {
                AccountHashedId = source.AccountHashedId
            });
        }
    }
}

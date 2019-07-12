using System.Collections.Generic;
using System.Text;
using MediatR;

namespace SFA.DAS.EmployerCommitmentsV2.Application.Queries.Providers
{
    public class GetProviderRequest : IRequest<GetProviderResponse>
    {
        public long UkPrn { get; set; }
    }
}

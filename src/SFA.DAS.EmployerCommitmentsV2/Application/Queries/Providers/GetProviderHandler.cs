using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.CommitmentsV2.Api.Client;

namespace SFA.DAS.EmployerCommitmentsV2.Application.Queries.Providers
{
    public class GetProviderHandler : IRequestHandler<GetProviderRequest, GetProviderResponse>
    {
        public Task<GetProviderResponse> Handle(GetProviderRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
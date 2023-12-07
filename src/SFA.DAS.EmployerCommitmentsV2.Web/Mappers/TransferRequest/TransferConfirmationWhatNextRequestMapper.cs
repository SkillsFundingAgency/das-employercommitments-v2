using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;
using SFA.DAS.EmployerUrlHelper;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.TransferRequest;

public class TransferConfirmationWhatNextRequestMapper : IMapper<TransferConfirmationViewModel, TransferConfirmationWhatNextRequest>
{
    private readonly ILinkGenerator _linkGenerator;

    public TransferConfirmationWhatNextRequestMapper(ILinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    public async Task<TransferConfirmationWhatNextRequest> Map(TransferConfirmationViewModel source)
    {
        return await Task.FromResult(new TransferConfirmationWhatNextRequest
        {
            WhatNextUrl = source.SelectedOption == TransferConfirmationViewModel.Option.Homepage
                ? _linkGenerator.EmployerHome(source.AccountHashedId)
                : _linkGenerator.EmployerFinanceTransfers(source.AccountHashedId)
        }); ;
    }
}
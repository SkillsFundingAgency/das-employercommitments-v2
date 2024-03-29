﻿using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class AttachUserInfoToSaveRequests<TFrom, TTo> : IMapper<TFrom, TTo> 
    where TFrom : class
    where TTo : class
{
    private readonly IMapper<TFrom, TTo> _innerMapper;
    private readonly IAuthenticationService _authenticationService;

    public AttachUserInfoToSaveRequests(IMapper<TFrom, TTo> innerMapper, IAuthenticationService authenticationService)
    {
        _innerMapper = innerMapper;
        _authenticationService = authenticationService;
    }

    public async Task<TTo> Map(TFrom source)
    {
        var to = await _innerMapper.Map(source);

        if (to is SaveDataRequest saveDataRequest)
        {
            saveDataRequest.UserInfo = _authenticationService.UserInfo;
        }
        return to;
    }

}
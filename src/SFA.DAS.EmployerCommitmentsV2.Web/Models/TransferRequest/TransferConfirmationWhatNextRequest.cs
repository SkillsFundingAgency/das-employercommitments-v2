﻿using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;

public class TransferConfirmationWhatNextRequest : IAuthorizationContextModel
{   
    public string WhatNextUrl { get; set; }
}
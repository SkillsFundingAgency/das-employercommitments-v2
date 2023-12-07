using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;

public class TransferConfirmationWhatNextRequest : IAuthorizationContextModel
{   
    public string WhatNextUrl { get; set; }
}
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class WhoWillEnterTheDetailsRequest : IAuthorizationContextModel
{
    [FromRoute]
    public string AccountHashedId { get; set; }
    [FromRoute]
    public string ApprenticeshipHashedId { get; set; }

    public long ProviderId { get; set; }
}
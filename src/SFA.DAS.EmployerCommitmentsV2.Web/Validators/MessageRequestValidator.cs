﻿using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class MessageRequestValidator : AbstractValidator<MessageRequest>
{
    public MessageRequestValidator()
    {
        RuleFor(x => x.ProviderId).GreaterThan(0);
    }
}
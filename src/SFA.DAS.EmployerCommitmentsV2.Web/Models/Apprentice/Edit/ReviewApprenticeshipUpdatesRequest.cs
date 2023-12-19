﻿namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

public class ReviewApprenticeshipUpdatesRequest : IAuthorizationContextModel
{
    [FromRoute]
    public string AccountHashedId { get; set; }

    public long AccountId { get; set; }

    [FromRoute]
    public string ApprenticeshipHashedId { get; set; }

    public long ApprenticeshipId { get; set; }
}
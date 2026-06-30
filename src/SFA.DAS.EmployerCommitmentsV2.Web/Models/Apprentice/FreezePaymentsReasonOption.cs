namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class FreezePaymentsReasonOption
{
    public int Value { get; init; }
    public string DisplayText { get; init; }

    public static IReadOnlyCollection<FreezePaymentsReasonOption> All { get; } =
    [
        new() { Value = 1, DisplayText = "Learner is on a break" },
        new() { Value = 2, DisplayText = "Learner has withdrawn" },
        new() { Value = 3, DisplayText = "There is a change to training details" },
        new() { Value = 4, DisplayText = "You disagree with an auto approved change" }
    ];
}

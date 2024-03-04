namespace SFA.DAS.EmployerCommitmentsV2.Contracts;

public interface ICommitmentsAuthorisationHandler
{
    Task<bool> CanAccessCohort();
    Task<bool> CanAccessApprenticeship();
}
namespace SFA.DAS.EmployerCommitmentsV2.Exceptions;

public class CohortEmployerUpdateDeniedException : UnauthorizedAccessException
{
    public CohortEmployerUpdateDeniedException(string message) :base(message)
    {}
}
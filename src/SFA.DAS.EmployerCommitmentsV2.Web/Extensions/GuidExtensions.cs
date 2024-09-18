namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

public static class GuidExtensions
{
    /// <summary>
    /// Checks if a nullable Guid is not null and not empty (Guid.Empty).
    /// </summary>
    /// <param name="guid">The nullable Guid to check.</param>
    /// <returns>True if the Guid is not null and not Guid.Empty, otherwise false.</returns>
    public static bool IsNotNullOrEmpty(this Guid? guid)
    {
        return guid.HasValue && guid.Value != Guid.Empty;
    }
}

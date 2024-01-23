namespace SFA.DAS.EmployerCommitmentsV2.Configuration;

public static class ConfigurationKeys
{
    public const string Encoding = "SFA.DAS.Encoding";
    public const string EmployerUrlConfiguration = "SFA.DAS.EmployerUrlHelper";
    public const string GovUkSignInConfiguration = "SFA.DAS.Employer.GovSignIn";

    public const string EmployerCommitmentsV2 = "SFA.DAS.EmployerCommitmentsV2";
    
    public const string AuthenticationConfiguration = $"{EmployerCommitmentsV2}:AuthenticationConfiguration";
    public const string CommitmentsApiClientConfiguration = $"{EmployerCommitmentsV2}:CommitmentsApiClientConfiguration";
    public const string EmployerFeaturesConfiguration = $"{EmployerCommitmentsV2}:Features";
    public const string AccountApiConfiguration = $"{EmployerCommitmentsV2}:AccountApi";
    public const string ZenDeskConfiguration = $"{EmployerCommitmentsV2}:ZenDesk";
    public const string ConnectionStrings = $"{EmployerCommitmentsV2}:ConnectionStrings";
    public const string ApprovalsApiClientConfiguration = $"{EmployerCommitmentsV2}:ApprovalsApiClientConfiguration";
  
}
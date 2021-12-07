namespace SFA.DAS.EmployerCommitmentsV2.Configuration
{
    public static class ConfigurationKeys
    {
        public const string Encoding = "SFA.DAS.Encoding";
        public static readonly string AccountIdHashingConfiguration = $"{EmployerCommitmentsV2}:AccountIdHashingConfiguration";
        public static readonly string AuthenticationConfiguration = $"{EmployerCommitmentsV2}:AuthenticationConfiguration";
        public static readonly string CommitmentsApiClientConfiguration = $"{EmployerCommitmentsV2}:CommitmentsApiClientConfiguration";
        public const string EmployerCommitmentsV2 = "SFA.DAS.EmployerCommitmentsV2";
        public static readonly string EmployerFeaturesConfiguration = $"{EmployerCommitmentsV2}:Features";
        public static readonly string AccountApiConfiguration = $"{EmployerCommitmentsV2}:AccountApi";
        public static readonly string PublicAccountIdHashingConfiguration = $"{EmployerCommitmentsV2}:PublicAccountIdHashingConfiguration";
        public static readonly string PublicAccountLegalEntityIdHashingConfiguration = $"{EmployerCommitmentsV2}:PublicAccountLegalEntityIdHashingConfiguration";
        public const string EmployerUrlConfiguration = "SFA.DAS.EmployerUrlHelper";
        public const string EmployerSharedUiConfiguration = "SFA.DAS.Employer.Shared.UI";
        public static readonly string ZenDeskConfiguration = $"{EmployerCommitmentsV2}:ZenDesk";
        public static readonly string ConnectionStrings = $"{EmployerCommitmentsV2}:ConnectionStrings";
        public static readonly string ApprovalsApiClientConfiguration = $"{EmployerCommitmentsV2}:ApprovalsApiClientConfiguration";
    }
}
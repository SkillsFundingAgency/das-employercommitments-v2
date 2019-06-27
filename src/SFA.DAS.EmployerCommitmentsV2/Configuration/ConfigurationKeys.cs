namespace SFA.DAS.EmployerCommitmentsV2.Configuration
{
    public static class ConfigurationKeys
    {
        public static readonly string AccountIdHashingConfiguration = $"{EmployerCommitmentsV2}:AccountIdHashingConfiguration";
        public static readonly string AuthenticationConfiguration = $"{EmployerCommitmentsV2}:AuthenticationConfiguration";
        public static readonly string CommitmentsApiClientConfiguration = $"{EmployerCommitmentsV2}:CommitmentsApiClientConfiguration";
        public const string EmployerCommitmentsV2 = "SFA.DAS.EmployerCommitmentsV2";
        public const string Encoding = "SFA.DAS.Encoding";
        public static readonly string PublicAccountIdHashingConfiguration = $"{EmployerCommitmentsV2}:PublicAccountIdHashingConfiguration";
        public static readonly string PublicAccountLegalEntityIdHashingConfiguration = $"{EmployerCommitmentsV2}:PublicAccountLegalEntityIdHashingConfiguration";
    }
}
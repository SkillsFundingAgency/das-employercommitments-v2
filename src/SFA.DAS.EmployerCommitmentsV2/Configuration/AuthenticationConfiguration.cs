namespace SFA.DAS.EmployerCommitmentsV2.Configuration
{
    public class AuthenticationConfiguration
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string LogoutEndpoint { get; set; }
        public string MetadataAddress { get; set; }
    }
}
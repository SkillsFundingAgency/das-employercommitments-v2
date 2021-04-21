using Microsoft.Extensions.Configuration;
using SFA.DAS.Authorization.CommitmentPermissions.Configuration;
using SFA.DAS.Authorization.EmployerFeatures.Configuration;
using SFA.DAS.CommitmentsV2.Api.Client.Configuration;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.Encoding;
using SFA.DAS.Http.Configuration;
using StructureMap;

namespace SFA.DAS.EmployerCommitmentsV2.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            AddConfiguration<AccountIdHashingConfiguration>(ConfigurationKeys.AccountIdHashingConfiguration);
            AddConfiguration<AuthenticationConfiguration>(ConfigurationKeys.AuthenticationConfiguration);
            AddConfiguration<CommitmentsClientApiConfiguration>(ConfigurationKeys.CommitmentsApiClientConfiguration);
            AddConfiguration<CommitmentPermissionsApiClientConfiguration>(ConfigurationKeys.CommitmentsApiClientConfiguration);
            AddConfiguration<EmployerCommitmentsV2Configuration>(ConfigurationKeys.EmployerCommitmentsV2);
            AddConfiguration<EmployerFeaturesConfiguration>(ConfigurationKeys.EmployerFeaturesConfiguration);
            AddConfiguration<PublicAccountIdHashingConfiguration>(ConfigurationKeys.PublicAccountIdHashingConfiguration);
            AddConfiguration<PublicAccountLegalEntityIdHashingConfiguration>(ConfigurationKeys.PublicAccountLegalEntityIdHashingConfiguration);
            AddConfiguration<EncodingConfig>(ConfigurationKeys.Encoding);
            AddConfiguration<AccountApiConfiguration>(ConfigurationKeys.AccountApiConfiguration);
            AddConfiguration<ZenDeskConfiguration>(ConfigurationKeys.ZenDeskConfiguration);
        }

        private void AddConfiguration<T>(string key) where T : class
        {
            For<T>().Use(c => GetConfiguration<T>(c, key)).Singleton();
        }

        private T GetConfiguration<T>(IContext context, string name)
        {
            var configuration = context.GetInstance<IConfiguration>();
            var section = configuration.GetSection(name);
            var value = section.Get<T>();

            return value;
        }
    }
}
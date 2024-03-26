using SFA.DAS.EmployerCommitmentsV2.Features;

namespace SFA.DAS.EmployerCommitmentsV2.Configuration;
public interface IFeaturesConfiguration<T> where T : FeatureToggle, new()
{
    List<T> FeatureToggles { get; }
}

public class EmployerFeaturesConfiguration : IFeaturesConfiguration<EmployerFeatureToggle>
{
    public List<EmployerFeatureToggle> FeatureToggles { get; set; }
}
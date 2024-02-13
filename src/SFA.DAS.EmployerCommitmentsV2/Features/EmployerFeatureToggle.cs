using System.Text.RegularExpressions;

namespace SFA.DAS.EmployerCommitmentsV2.Features;

public class EmployerFeatureToggle : FeatureToggle
{
    public List<long> AccountWhitelist { get; set; } = new();
    public List<string> EmailWhitelist { get; set; } = new();
    public long? EnabledByAgreementVersion { get; set; }

    public bool IsWhitelistEnabled => IsEmailWhitelistEnabled || IsAccountWhitelistEnabled;
        
    private bool IsEmailWhitelistEnabled => EmailWhitelist != null && EmailWhitelist.Count > 0;
    private bool IsAccountWhitelistEnabled => AccountWhitelist != null && AccountWhitelist.Count > 0;

    public bool IsUserWhitelisted(long? accountId, string userEmail) =>
        IsAccountWhitelisted(accountId) || IsEmailWhitelisted(userEmail);

    private bool IsAccountWhitelisted(long? accountId)
        => IsAccountWhitelistEnabled && accountId.HasValue && AccountWhitelist.Contains(accountId.Value);

    private bool IsEmailWhitelisted(string userEmail)
    {
        if(!IsEmailWhitelistEnabled || string.IsNullOrWhiteSpace(userEmail))
        {
            return false;
        }

        var wildcards = EmailWhitelist.Where(s => s.Contains('*')).ToList();
        var emails = EmailWhitelist.Except(wildcards);

        return emails.Any(email => email.Equals(userEmail, StringComparison.InvariantCultureIgnoreCase))
               || wildcards.Any(pattern => Regex.IsMatch(userEmail, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(10)));
    }
}
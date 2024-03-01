namespace SFA.DAS.EmployerCommitmentsV2.Configuration;

public abstract class HashingConfiguration
{
    public string Alphabet { get; set; }
    public string Salt { get; set; }
}

public class AccountIdHashingConfiguration : HashingConfiguration
{
}

public class PublicAccountIdHashingConfiguration : HashingConfiguration
{
}

public class PublicAccountLegalEntityIdHashingConfiguration : HashingConfiguration
{
}
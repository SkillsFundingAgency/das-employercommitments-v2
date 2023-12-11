namespace SFA.DAS.EmployerCommitmentsV2.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class ReferenceMetadataAttribute : Attribute
{
    public string Description { get; set; }
    public string Hint { get; set; }
}
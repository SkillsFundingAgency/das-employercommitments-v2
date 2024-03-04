namespace SFA.DAS.EmployerCommitmentsV2.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class SuppressArgumentExceptionAttribute : Attribute
{
    public string PropertyName { get; }
    public string CustomErrorMessage { get; }

    public SuppressArgumentExceptionAttribute(string propertyName, string customErrorMessage)
    {
        PropertyName = propertyName;
        CustomErrorMessage = customErrorMessage;
    }
}
using System;
using SFA.DAS.EmployerCommitmentsV2.Attributes;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class ReferenceMetadataExtensions
    {
        public static string GetReferenceDataDescription(this Enum value)
        {
            var displayAttribute = GetMetadataAttribute(value);
            return displayAttribute == null ? value.ToString() : displayAttribute.Description;
        }

        public static string GetReferenceDataHint(this Enum value)
        {
            var displayAttribute = GetMetadataAttribute(value);
            return displayAttribute == null ? "" : displayAttribute.Hint;
        }

        private static ReferenceMetadataAttribute GetMetadataAttribute(Enum value)
        {
            var type = value.GetType();

            var members = type.GetMember(value.ToString());
            if (members.Length == 0) throw new ArgumentException($"error '{value}' not found in type '{type.Name}'");

            var member = members[0];
            var attributes = member.GetCustomAttributes(typeof(ReferenceMetadataAttribute), false);
            if (attributes.Length > 0)
            {
                return (ReferenceMetadataAttribute)attributes[0];
            }

            return null;
        }
    }
}

using System;

namespace SFA.DAS.EmployerCommitmentsV2.Attributes
{
    public class ReferenceMetadataAttribute : Attribute
    {
        public string Description { get; set; }
        public string Hint { get; set; }
    }
}

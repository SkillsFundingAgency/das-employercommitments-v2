using SFA.DAS.Encoding;
using StructureMap;

namespace SFA.DAS.EmployerCommitmentsV2.Web.DependencyResolution
{
    public class EncodingRegistry : Registry
    {
        public EncodingRegistry()
        {
            For<IEncodingService>().Use<EncodingService>().Singleton();
        }
    }
}
using SFA.DAS.Encoding;
using StructureMap;

namespace SFA.DAS.Commitments.Shared.DependencyInjection
{
    public class _EncodingRegistry : Registry
    {
        public _EncodingRegistry()
        {
            For<IEncodingService>().Use<EncodingService>().Singleton();
        }
    }
}
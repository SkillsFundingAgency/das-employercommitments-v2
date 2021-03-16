using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Web.DependencyResolution;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Remove
{
    public class DummyRegistry : Registry
    {
        public DummyRegistry()
        {
            For<ICommitmentsApiClientFactory>().ClearAll().Use<CommitmentsApiClientFactory2>();
        }
    }
}

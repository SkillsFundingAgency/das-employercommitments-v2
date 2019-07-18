using System;
using StructureMap;

namespace SFA.DAS.Encoder.DependencyResolution
{
    public static class IoC
    {
        public static IContainer InitializeIoC(Action<ConfigurationExpression> builder)
        {
            return new Container(c =>
            {
                c.AddRegistry<DefaultRegistry>();
                c.AddRegistry<ConfigurationRegistry>();
                c.Scan(s => s.AddAllTypesOf<ICommand>());
                builder(c);
            });
        }
    }
}
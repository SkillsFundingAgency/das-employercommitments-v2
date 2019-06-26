using System;
using Microsoft.Extensions.Options;

namespace SFA.DAS.EmployerCommitmentsV2.Web.DependencyResolution
{
    /// <summary>
    ///     Projects a static config object into an IOptionsMonitor for the config object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StaticOptionsMonitor<T> : IOptionsMonitor<T>
    {
        public StaticOptionsMonitor(T instance)
        {
            CurrentValue = instance;
        }

        public T CurrentValue { get; }

        public T Get(string name)
        {
            throw new NotImplementedException();
        }

        public IDisposable OnChange(Action<T, string> listener)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Diagnostics;
using System.Reflection;

namespace DevexpApiSdk.Common.Metrics
{
    internal static class ProfilingProxyFactory
    {
        internal static T Create<T>(T inner, DevexpApiOptions options)
            where T : class
        {
            var proxy = DispatchProxy.Create<T, ProfilingProxy<T>>();
            var impl = (ProfilingProxy<T>)(object)proxy!;
            impl.Inner = inner;
            impl.Options = options;
            return proxy;
        }
    }
}

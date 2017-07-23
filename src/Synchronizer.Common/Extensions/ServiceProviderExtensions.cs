using System;

namespace Synchronizer.Common.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static T Resolve<T>(this IServiceProvider sp) => (T)sp.GetService(typeof(T));
    }
}
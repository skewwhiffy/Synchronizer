using System;

namespace Synchronizer.Common.Extensions
{
    public static class FunctionalExtensions
    {
        public static TOut Pipe<TIn, TOut>(this TIn source, Func<TIn, TOut> map) => map(source);

        public static T Pipe<T>(this T source, Action<T> action)
        {
            action(source);
            return source;
        }
    }
}

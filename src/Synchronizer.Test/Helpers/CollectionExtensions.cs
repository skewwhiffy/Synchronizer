using System;
using System.Collections.Generic;
using System.Linq;

namespace Synchronizer.Test.Helpers
{
    public static class CollectionExtensions
    {
        public static T TakeRandom<T>(this IEnumerable<T> source)
        {
            var random = new Random();
            var sourceList = (source as IList<T>) ?? source.ToList();
            return sourceList[random.Next(sourceList.Count)];
        }
    }
}
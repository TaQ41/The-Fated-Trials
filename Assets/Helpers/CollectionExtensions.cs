using System.Collections.Generic;

#pragma warning disable IDE0130

namespace Helpers
{
    /// <summary>
    /// A group of methods to provide more extensible control over collections easily.
    /// </summary>
    public static class CollectionExtensions
    {
        public static IEnumerable<string> SuffixItems(this IEnumerable<string> collection, string suffix)
        {
            foreach (string item in collection)
                yield return item + suffix;
        }
    }
}
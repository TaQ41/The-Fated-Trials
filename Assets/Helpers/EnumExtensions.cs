using System;
using System.Collections.Generic;

#pragma warning disable IDE0130

namespace Helpers
{
    /// <summary>
    /// A group of methods to provide more extensive control over enums easily.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Convert a flagged enum into a collection of strings of each flagged member name.
        /// Use this to prevent members that include other members from culling the members that they wrap.
        /// (IE. C = A | B; this(C) = 'A', 'B' instead of 'C')
        /// </summary>
        /// <param name="flaggedEnum">The flagged enum being operated on.</param>
        /// <returns>A collection of strings of each flagged member. An empty collection if the only value was None.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<string> EnumFlagsToString<TEnum>(TEnum flaggedEnum) where TEnum : Enum
        {   
            #pragma warning disable IDE0047

            if (!(typeof(TEnum).IsDefined(typeof(FlagsAttribute), false)))
                throw new ArgumentException($"The enum must be a Flags enum. ({typeof(Enum).Name})");

            #pragma warning restore IDE0047

            List<string> flagNames = new();
            foreach (TEnum flag in Enum.GetValues(typeof(TEnum)))
            {
                // Exclude the member with the value '0', IE. 'None'
                if (flag.Equals(Enum.ToObject(typeof(TEnum), 0)))
                    continue;

                if (flaggedEnum.HasFlag(flag) && IsFlagSingleBit(flag))
                    flagNames.Add(flag.ToString());
            }

            return flagNames;
        }

        /// <summary>
        /// Determines if an enum flag's underlying bit value is a power of 2. (Only a single 1 in a binary number. 1000 0100 0010 0001)
        /// This proves whether the flag is a single bit or not (eg. excludes flags that are c = a | b).
        /// Uses a common bitwise trick to determine that proof.z
        /// </summary>
        /// <param name="flag">The flag tested to determine if it is a single bit value.</param>
        /// <returns>Whether or not the provided flag value was a single bit.</returns>
        private static bool IsFlagSingleBit(Enum flag)
        {
            // Omits the flagValue > 0 as this is already known to be true.
            // Add as a logical AND to the return if new methods arise that don't gaurantee this behavior.
            long flagValue = Convert.ToInt64(flag);
            return (flagValue & (flagValue - 1)) == 0;
        }
    }
}
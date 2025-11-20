using System;
using System.Collections.Generic;
using Helpers;

namespace GenericSceneLoader.Locale
{

    public static class LocaleFinder
    {
        public const string VillageLocaleSceneNameSuffix = " Village Locale";
        
        /// <summary>
        /// Represents the village locales in the game.
        /// </summary>
        [Flags]
        public enum VillageLocales
        {
            None = 0,
            Main = 1,
            All = Main
        };
    
        /// <summary>
        /// Get the full name of a village locale scene. Intended for single values only.
        /// </summary>
        /// <param name="villageLocale">The single village locale to be used.</param>
        /// <returns>A string of the village locale scene name in Unity.</returns>
        public static string GetVillageLocaleSceneName(VillageLocales villageLocale)
        {
            return villageLocale.ToString() + VillageLocaleSceneNameSuffix;
        }
    
        /// <summary>
        /// Get the full scene name of all selected village locales. Intended for single and multiple values.
        /// </summary>
        /// <param name="villageLocales">The village locales to be used.</param>
        /// <returns>An IEnumerable of string scene names of the village locales in Unity.</returns>
        public static IEnumerable<string> GetMultipleVillageLocaleSceneNames(VillageLocales villageLocales)
        {
            return EnumExtensions.EnumFlagsToString(villageLocales).SuffixItems(VillageLocaleSceneNameSuffix);
        }
    }
}
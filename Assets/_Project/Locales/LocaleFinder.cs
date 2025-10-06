using System;
using System.Collections.Generic;
using static Helpers.EnumExtensions;
using static Helpers.CollectionExtensions;

public static class LocaleFinder
{
    public const string VillageLocaleSceneNameSuffix = " Village Locale";
    /// <summary>
    /// Represents the village locales in the game. Flagged as certain fields may require this to be a multi-selection.
    /// </summary>
    [Flags]
    public enum VillageLocales
    {
        None = 0,
        Main = 1,
    };

    public static IEnumerable<string> GetVillageLocaleSceneNames(VillageLocales villageLocales)
        => EnumFlagsToString(villageLocales).SuffixItems(VillageLocaleSceneNameSuffix);
}
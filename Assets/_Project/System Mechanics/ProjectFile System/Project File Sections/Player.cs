using UnityEngine;

namespace ProjectFile
{
    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class Player
    {
        public string Name;

        /// <summary>
        /// The current village locale of the player. Only one flag should be set at a time.
        /// If more than one are set, the first one found will be chosen. Defaults to 'Main' if 'None' is set.
        /// </summary>
        public LocaleFinder.VillageLocales CurrentVillageLocale;
    }
}
using UnityEngine;

namespace ProjectFileSystem
{
    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class Player
    {
        public string Name;

        /// <summary>
        /// The current village locale scene name of the player. Use the single village locale scene name method in the LocaleFinder.
        /// </summary>
        public string CurrentVillageLocaleSceneName;
    }
}
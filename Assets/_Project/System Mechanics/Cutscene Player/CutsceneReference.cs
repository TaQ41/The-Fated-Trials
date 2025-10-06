namespace CutscenePlayer
{
    /// <summary>
    /// Contains values that specify the cutscene to play when its constraints pass.
    /// Current Constraints:
    /// * PlayableLocales - In which villages the cutscene may be played.
    /// </summary>
    [System.Serializable]
    public struct CutsceneReference
    {
        /// <summary>
        /// The key to use when fetching the cutscene.
        /// </summary>
        public string CutsceneName;

        /// <summary>
        /// The village locales that this cutscene can be played at, if this constraint isn't met, the cutscene shouldn't play.
        /// </summary>
        public LocaleFinder.VillageLocales PlayableLocales;
    }
}
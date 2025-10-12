using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GenericSceneLoader.Locale
{
    public class LocaleBootup : MonoBehaviour
    {
        const string GenericLocaleLoaderSceneName = "Generic Locale Loader";
        const string LocaleBootupObjectTag = "Locale Bootup";

        [SerializeField]
        private CutsceneSystem.CutscenePlayer m_cutscenePlayer;

        private string m_sceneName;
    
        /// <summary>
        /// Load the Generic Locale Loader scene and begin the process for loading the scene that
        /// has been passed into this after playing cutscenes. It is recommended to disable input before calling this.
        /// </summary>
        /// <param name="sceneName">The locale scene to load into.</param>
        public static async Task BootupLocale(string sceneName)
        {
            await SceneManager.LoadSceneAsync(GenericLocaleLoaderSceneName, LoadSceneMode.Single);
            GameObject.FindWithTag(LocaleBootupObjectTag)
                      .GetComponent<LocaleBootup>()
                      .PreLoadLocale(sceneName);
        }

        private void PreLoadLocale(string sceneName)
        {
            m_sceneName = sceneName;
            m_cutscenePlayer.UseInitialCutscenes();
            m_cutscenePlayer.completed += UnloadBootup;
            m_cutscenePlayer.Play();
        }

        private void UnloadBootup()
        {
            m_cutscenePlayer.completed -= UnloadBootup;
            SceneManager.LoadSceneAsync(m_sceneName, LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync(gameObject.scene);
        }
    }
}
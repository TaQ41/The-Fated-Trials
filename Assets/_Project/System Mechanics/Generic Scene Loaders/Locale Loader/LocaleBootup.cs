using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GenericSceneLoader.Locale
{

public class LocaleBootup : MonoBehaviour
{
    const string GenericLocaleLoaderSceneName = "Generic Locale Loader";
    const string LocaleBootupObjectTag = "Locale Bootup";

    /// <summary>
    /// Load the Generic Locale Loader scene and begin the process for loading the scene that
    /// has been passed into this after playing cutscenes. It is recommended to disable input before calling this.
    /// </summary>
    /// <param name="sceneName">The locale scene to load into.</param>
    public static async Task BootupLocale(string sceneName)
    {
        await SceneManager.LoadSceneAsync(GenericLocaleLoaderSceneName, LoadSceneMode.Single);
        try
        {
            GameObject.FindWithTag(LocaleBootupObjectTag)
                          .GetComponent<LocaleBootup>()
                              .PreLoadLocale(sceneName);
        }
        catch
        {
            Debug.LogError("Couldn't find the locale bootup object by its tag!");
        }
    }

    private void PreLoadLocale(string sceneName)
    {
        Debug.Log("No cutscene player implemented!");

        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }
}
}
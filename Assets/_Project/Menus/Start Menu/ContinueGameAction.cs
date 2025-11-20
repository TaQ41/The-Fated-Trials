using ProjectFileSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using GenericSceneLoader.Locale;

namespace GameIOActions
{

    /// <summary>   
    /// This will be merged with all the other actions later to create a far more extensible and contained logic.
    /// </summary>
    public class ContinueGameAction : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private ProjectFileHeader projectFile;
    
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            ProjectFileUtilities.DeserializeFile(projectFile.IdentificationData.Guid, projectFile);
            if (projectFile.PlayerData.CurrentVillageLocaleSceneName == LocaleFinder.GetVillageLocaleSceneName(LocaleFinder.VillageLocales.None))
                projectFile.PlayerData.CurrentVillageLocaleSceneName = LocaleFinder.GetVillageLocaleSceneName(LocaleFinder.VillageLocales.Main);
    
            _ = LocaleBootup.BootupLocale(projectFile.PlayerData.CurrentVillageLocaleSceneName);
        }
    
    }
}
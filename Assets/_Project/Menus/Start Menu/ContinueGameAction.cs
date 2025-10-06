using System.Collections;
using System.Linq;
using ProjectFile;
using UnityEngine;
using UnityEngine.EventSystems;
using static GenericSceneLoader.Locale.LocaleBootup;

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
    }
}
}
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace ProjectFile
{

/// <summary>
/// A class that is attached to UI elements to give a button-like functionality that commits specified actions when clicked.
/// This exhibits constraints ... <- Fill in these. This is a prototype! functionalities should not be taken as permanent and changed when the structure
/// different home maps change and/or when a dedicated transition manager is implemented.
/// </summary>
public class ProjectFileActions : MonoBehaviour, IPointerClickHandler
{
    private const string StartMenuScenePath = "Start Menu";
    private const string HomeMapScenePath = "Home Map";

    // InfoLog is an unspecified action as of now, it will essentially pull up all information about the projectFile.
    public enum Actions
    {
        Unset     = 0,
        Save      = 1,
        Save_Exit = 2,
        Load      = 4,
        Delete    = 8,
        InfoLog   = 16,
    };

    [SerializeField]
    private ProjectFileHeader projectFile;

    [SerializeField, Tooltip("What action should this button do?")]
    private Actions action;

    /// <summary>
    /// A property that sends the guid parameter to methods in the ProjectFileUtilities when they require it.
    /// Actions like 'Save' and 'Save_Exit' will assume the current projectFile's guid.
    /// </summary>
    public string SelectedGuid { get; set; } = string.Empty;

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("ProjectFile Action occured in a limited prototype.");
        switch (action)
        {
            case Actions.Save:
                _ = ProjectFileUtilities.SerializeFile(projectFile);
                break;
            case Actions.Save_Exit:
                _ = ProjectFileUtilities.SerializeFile(projectFile); // This will later be under a common method that actually does something.
                SceneManager.LoadScene(StartMenuScenePath);
                break;
            case Actions.Load:
                ProjectFileUtilities.DeserializeFile(projectFile.IdentificationData.Guid, projectFile);
                SceneManager.LoadScene(HomeMapScenePath);
                break;
            case Actions.Delete:
                ProjectFileUtilities.DeleteFile(SelectedGuid);
                projectFile = null;
                break;
            default:
                Debug.Log("No specified action.");
                break;
        }
    }
}
}
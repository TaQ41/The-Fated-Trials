using System.Threading.Tasks;
using ProjectFileSystem;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProjectFileUtilsTesting : MonoBehaviour
{
    [SerializeField]
    private ProjectFileHeader projFile;

    [Button]
    public async Task SaveFile()
    {
        bool result = await ProjectFileUtilities.SerializeFile(projFile);
        Debug.Log("result: " + result);
    }

    [Button]
    public void GetFile(string guid)
    {
        Debug.Log("Ran!");
        ProjectFileUtilities.DeserializeFile(guid, projFile);
    }

    [Button]
    public void DeleteFile1(string guid)
    {
        ProjectFileUtilities.DeleteFile(guid);
        Debug.Log("Attempted to delete the file by guid: " + guid + "!");
    }

    [Button]
    public void PullAll()
    {
        QuickPullObject[] quickPull = ProjectFileUtilities.QuickPullAll();
        foreach (QuickPullObject obj in quickPull)
        {
            Debug.Log(obj.ProjectName + $"\n{obj.Guid}");
        }
    }

    [Button]
    public void GenerateQuickPullData()
    {
        ProjectFileUtilities.RegenerateQuickPullData();
    }
}

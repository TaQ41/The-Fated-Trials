using System.Threading.Tasks;
using ProjectFile;
using Sirenix.OdinInspector;
using UnityEngine;
using static ProjectFile.ProjectFileUtilities;

public class ProjectFileUtilsTesting : MonoBehaviour
{
    [SerializeField]
    private ProjectFileHeader projFile;

    [Button]
    public void PrintAllMinimal()
    {

        MinimalFileInfo[] myMFIs = GetMinimalFileInfos();
        foreach (var mfi in myMFIs)
            Debug.Log("guid: " + mfi.Guid + "\nname: " + mfi.ProjectName);
    }

    [Button]
    public async Task SaveFile()
    {
        bool result = await SerializeFile(projFile);
        Debug.Log("result: " + result);
    }

    [Button]
    public void GetFile(string guid)
    {
        Debug.Log("Ran!");
        DeserializeFile(guid, projFile);
    }

    [Button]
    public void DeleteFile1(string guid)
    {
        DeleteFile(guid);
        Debug.Log("Attempted to delete the file by guid: " + guid + "!");
    }

    [Button]
    public void ResetMIS_Data()
    {
        GenerateMIS_DataFile();
    }
}

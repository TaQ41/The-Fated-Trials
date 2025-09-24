using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectFile
{
    public static class ProjectFileUtilities
    {
        // Constants
        private const string SavedFilesDir = @"Assets\_Project\ProjectFile System\Saved Files";
        private const string MIS_FilePath = SavedFilesDir + @"\MIS_Data.json";

        /// <summary>
        /// The wrapper class to allow serialization and deserialization of a list of MinimalFileInfo structs
        /// used by the MIS methods. May be replaced with a global listWrapper later on.
        /// </summary>
        [Serializable]
        private class MFI_Wrapper
        {
            public List<MinimalFileInfo> MinimalFileInfos = new();

            public MinimalFileInfo this[int index] { set {MinimalFileInfos[index] = value;} }
            public int IndexOf(MinimalFileInfo mfi) => MinimalFileInfos.IndexOf(mfi);
            public void Add(MinimalFileInfo mfi) => MinimalFileInfos.Add(mfi);
            public void RemoveAt(int index) => MinimalFileInfos.RemoveAt(index);
            public int Count { get {return MinimalFileInfos.Count;} }
        }

        /// <summary>
        /// Updates the MIS_Data file to align with the changed Saved file. This can either
        /// delete a value or update its properties.
        /// </summary>
        /// <param name="id">The identification section of the projectFile.</param>
        /// <param name="isDeletion">Does this delete or update properties?</param>
        private static void UpdateMISData(Identification id, bool isDeletion)
        {
            string file = File.ReadAllText(MIS_FilePath);
            MFI_Wrapper MFIwrapper = JsonUtility.FromJson<MFI_Wrapper>(file);
            Debug.Log(MFIwrapper.MinimalFileInfos);

            int mfiIndex = 1;
            foreach (MinimalFileInfo mfi in MFIwrapper.MinimalFileInfos)
            {
                if (mfi.Guid == id.Guid)
                    break;

                mfiIndex++;
            }

            if (isDeletion)
            {
                if (mfiIndex == MFIwrapper.Count)
                    return;

                MFIwrapper.RemoveAt(mfiIndex - 1);
            }
            else
            {
                MinimalFileInfo replacementMFI = CreateMFI(id);

                if (mfiIndex == MFIwrapper.Count)
                    MFIwrapper.Add(replacementMFI);
                else
                    MFIwrapper[mfiIndex - 1] = replacementMFI;
            }

            File.WriteAllText(MIS_FilePath, JsonUtility.ToJson(MFIwrapper));
        }

        private static MinimalFileInfo CreateMFI(Identification id)
        {
            return new()
            {
                Guid = id.Guid,
                ProjectName = id.ProjectName
            };
        }

        public static void GenerateMIS_DataFile()
        {
            MFI_Wrapper MFIwrapper = new();
            ProjectFileHeader projFile = ScriptableObject.CreateInstance<ProjectFileHeader>();
            foreach (string file in Directory.EnumerateFiles(SavedFilesDir, searchPattern: "*.json"))
            {
                if (file.Equals("MIS_Data.json"))
                    continue;
                
                try {
                    JsonUtility.FromJsonOverwrite(File.ReadAllText(file), projFile);
                } catch {
                    continue;
                }

                MFIwrapper.Add(CreateMFI(projFile.Identification));
            }

            string MIS_DataJson = JsonUtility.ToJson(MFIwrapper);
            File.WriteAllText(MIS_FilePath, MIS_DataJson);
        }

        public static MinimalFileInfo[] GetMinimalFileInfos()
        {
            string file = File.ReadAllText(MIS_FilePath);
            return JsonUtility.FromJson<MFI_Wrapper>(file).MinimalFileInfos.ToArray();
        }

        /// <summary>
        /// Serialize a projectFile into a json file at its specified location (guid).
        /// Awaits the writing call.
        /// </summary>
        /// <param name="projectFile">The projectFile to save.</param>
        /// <returns>A bool determining if this succeeded or not.</returns>
        public static async Task<bool> SerializeFile(ProjectFileHeader projectFile)
        {
            try
            {
                if (!Directory.Exists(SavedFilesDir))
                    return false;

                await File.WriteAllTextAsync(SavedFilesDir + "\\" + projectFile.Identification.Guid + ".json", JsonUtility.ToJson(projectFile));
            }
            catch
            {
                return false;
            }

            UpdateMISData(projectFile.Identification, isDeletion: false);
            return true;
        }

        /// <summary>
        /// Deserialize a projectFile from its guid and sets it to the provided projectFileHeader instance.
        /// </summary>
        /// <param name="guid">The guid in the indentification section of a projectFile.</param>
        public static void DeserializeFile(string guid, ProjectFileHeader projFile)
        {
            foreach (string file in Directory.EnumerateFiles(SavedFilesDir, searchPattern: "*.json"))
            {
                if (file[(file.LastIndexOf('\\') + 1)..^5].Equals(guid))
                    JsonUtility.FromJsonOverwrite(File.ReadAllText(file), projFile);
            }
        }

        /// <summary>
        /// Delete a file in the Saved Files directory by its guid name.
        /// Then, call the MIS_Data updater to delete the information found there.
        /// </summary>
        /// <param name="guid">The guid of the id of the saved file to be deleted.</param>
        public static void DeleteFile(string guid)
        {
            // Causes stack overflow, get fixed set of files and then delete those.
            List<string> paths = new();

            // This should handle the .json and .meta files.
            foreach (string file in Directory.EnumerateFiles(SavedFilesDir, searchPattern: guid))
                paths.Add(file);
            
            paths.ForEach((x) => {File.Delete(x);});
            UpdateMISData(id: null, isDeletion: true);
        }
    }
}
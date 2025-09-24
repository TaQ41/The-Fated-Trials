using UnityEngine;
using ProjectFile;

/// <summary>
/// The main entry point to all serialized data in a projectFile for the game.
/// </summary>
[CreateAssetMenu(fileName = "ProjectFileHeader", menuName = "Scriptable Objects/ProjectFileHeader")]
public class ProjectFileHeader : ScriptableObject
{
    public Identification Identification;
}
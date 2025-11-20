namespace ProjectFileSystem
{
    /// <summary>
    /// An object used for concise data collection of each project file.
    /// All fields will be used in the "Load Game" section when displaying all files.
    /// </summary>
    [System.Serializable]
    public struct QuickPullObject
    {
        public string Guid;
        public string ProjectName;

        public QuickPullObject(Identification id)
        {
            Guid = id.Guid;
            ProjectName = id.ProjectName;
        }
    }
}
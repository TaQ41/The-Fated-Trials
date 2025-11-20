using System;
using UnityEngine;

namespace ProjectFileSystem
{
    /// <summary>
    /// All information regarding the file and its required properties for utility methods.
    /// </summary>
    [Serializable]
    public class Identification
    {
        [SerializeField]
        private string m_guid = System.Guid.NewGuid().ToString();
        public string Guid { get {return m_guid;}}

        public string ProjectName;
    }
}
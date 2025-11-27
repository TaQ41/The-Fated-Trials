using UnityEngine;

namespace ItemsCore.Structure
{
    [System.Serializable]
    public class RuntimeMinimalItem : IItem
    {
        [SerializeField] protected string m_itemName;
        [SerializeField] protected string m_imagePath;

        public string ItemName => m_itemName;
        public string ImagePath => m_imagePath;

        public RuntimeMinimalItem(string _itemName, string _imagePath = "")
        {
            m_itemName = _itemName;
            m_imagePath = _imagePath;
            if (_imagePath == string.Empty)
            {
                // Run lookup on ItemName and get imagePath.
            }
        }
    }
}

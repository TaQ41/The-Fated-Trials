using UnityEngine;

namespace ItemsCore.Structure
{
    [System.Serializable]
    public class RuntimeStorageItem : RuntimeMinimalItem, IStorageItem
    {
        [SerializeField] private int m_maxCount;
        [SerializeField] private string m_itemType;

        public int MaxCount => m_maxCount;
        public string ItemType => m_itemType;

        public RuntimeStorageItem(string _itemName, int _maxCount, string _itemType,
            string _imagePath = "") : base(_itemName, _imagePath)
        {
            m_maxCount = _maxCount;
            m_itemType = _itemType;
        }
    }
}

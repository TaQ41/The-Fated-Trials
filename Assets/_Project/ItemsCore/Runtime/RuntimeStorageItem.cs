using Sirenix.OdinInspector;
using UnityEngine;

namespace ItemsCore.Structure
{
    [System.Serializable]
    public class RuntimeStorageItem : RuntimeMinimalItem, IStorageItem
    {
        [SerializeField] private int m_maxStack;
        [SerializeField] private string m_itemType;

        public int MaxStack => m_maxStack;
        public string ItemType => m_itemType;

        public RuntimeStorageItem(string _itemName, int _maxStack, string _itemType,
            string _imagePath = "") : base(_itemName, _imagePath)
        {
            m_maxStack = _maxStack;
            m_itemType = _itemType;
        }
    }
}

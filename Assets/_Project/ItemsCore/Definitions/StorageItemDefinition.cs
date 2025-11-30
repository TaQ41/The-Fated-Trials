using UnityEngine;

namespace ItemsCore.Structure
{
    [CreateAssetMenu(fileName = "StorageItemDefinition", menuName = "Items/StorageItemDefinition")]
    public class StorageItemDefinition : ItemDefinition, IStorageItem
    {
        [SerializeField] private int m_maxStack;
        [SerializeField] private string m_itemType;

        public int MaxStack => m_maxStack;
        public string ItemType => m_itemType;

        public override IItem CreateRuntimeItem()
        {
            return new RuntimeStorageItem(_itemName: m_itemName, _maxStack: m_maxStack, _itemType: m_itemType,
                _imagePath: m_imagePath
            );
        }
    }
}
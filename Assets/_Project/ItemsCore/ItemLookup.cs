using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

// Parenthesis can be removed warning.
#pragma warning disable IDE0047

namespace ItemsCore.Structure
{
    [System.Serializable]
    class ItemDefinitionWrapper
    {
        [SerializeReference]
        public ItemDefinition WrappedItemDefinition;

        public ItemDefinitionWrapper(ItemDefinition item)
        {
            WrappedItemDefinition = item;
        }
    }

    [CreateAssetMenu(fileName = "ItemLookup", menuName = "Items/ItemLookup")]
    public class ItemLookup : SerializedScriptableObject
    {
        [SerializeField, DictionaryDrawerSettings(KeyLabel = "Item Name")]
        private Dictionary<string, ItemDefinitionWrapper> m_itemLookup = new();

        public (ItemDefinition item, bool success) Lookup(string itemName)
        {
            bool success = m_itemLookup.TryGetValue(itemName, out ItemDefinitionWrapper item);
            return (item.WrappedItemDefinition, success);
        }

        [Button]
        private void Add(string itemName, ItemDefinition item)
        {
            if (!(m_itemLookup.TryAdd(itemName, new ItemDefinitionWrapper(item))))
            {
                Debug.LogWarning($"Trying to add a key that already exists {itemName}");
            }
        }
    }
}
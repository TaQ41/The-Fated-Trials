using UnityEngine;
using System;
using System.Collections.Generic;

namespace InventorySystem
{
    [Serializable]
    public class StorageItemContext
    {
        public ItemsCore.IStorageItem Item;
        public int CurrStack;

        public string ItemName => Item.ItemName;
        public int MaxStack => Item.MaxStack;
    }

    public static class StorageItemContextExtensions
    {
        public static int AppendItemStack(this StorageItemContext itemContext, int count)
        {
            int maxAppendableAmount = itemContext.Item.MaxStack - itemContext.CurrStack;
            if (count > maxAppendableAmount)
            {
                itemContext.CurrStack = itemContext.MaxStack;
                return count - maxAppendableAmount;
            }
            
            itemContext.CurrStack += count;
            return 0;
        }
    }

    [Serializable]
    public class StorageItemBox1
    {
        [SerializeField]
        private List<StorageItemContext> m_items;

        public void Append(StorageItemContext itemContext)
        {
            Append(itemContext.Item, itemContext.CurrStack);
        }

        public void Append(in ItemsCore.IStorageItem item, int count)
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                StorageItemContext itemContext = m_items[i];
                if (itemContext.Item == default)
                {
                    itemContext.Item = item;
                    itemContext.CurrStack = 0;
                }
                else if (! itemContext.ItemName.Equals(item.ItemName))
                {
                    continue;
                }

                count = itemContext.AppendItemStack(count);
                if (count == 0)
                    return;
            }

            // Call loss manager for item overflow, typically this includes allowing the player to sell remaining items.
            // But if the player wanted to keep or swap out other items, they should be able to.
            // Nevermind, the player can still do that even after the elements have been appended to.
        }
    }
}
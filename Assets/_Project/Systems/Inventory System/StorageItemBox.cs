using System;
using System.Collections.Generic;
using ItemsCore.Structure;
using UnityEngine;

[Serializable]
public class StorageItemContext
{
    public string ItemName => Item.ItemName;
    public string ImagePath => Item.ImagePath;
    public int MaxStack => Item.MaxStack;
    
    public RuntimeStorageItem Item;
    public int ItemStack;
}

[Serializable]
public class StorageItemBox
{
    [SerializeField]
    private List<StorageItemContext> m_items;

    public void Append(StorageItemContext itemContext)
    {
        Append(itemContext.Item, itemContext.ItemStack);
    }

    public void Append(in RuntimeStorageItem item, int count)
    {
        Debug.Log("Ran");
        for (int i = 0; i < m_items.Count; i++)
        {
            Debug.Log($"Item {i}");
            StorageItemContext itemContext = m_items[i];

            if (!itemContext.ItemName.Equals(item.ItemName))
            {
                Debug.Log("Item names are not equal!");
                Debug.Log($"Is item set: {itemContext.Item == default}");
                if (itemContext.Item == default)
                {
                    itemContext.Item = item;
                    itemContext.ItemStack = 0;
                }
                else
                    continue;
            }
            
            int maxRequestableAmount = itemContext.MaxStack - itemContext.ItemStack;
            Debug.Log($"Max Stack of {itemContext.ItemName}: {itemContext.MaxStack}");
            Debug.Log($"Current Stack of item {i}: {itemContext.ItemStack}");
            Debug.Log("Max requestable amount: " + maxRequestableAmount);
            if (count > maxRequestableAmount)
            {
                Debug.Log("Count is above request amount");
                count -= maxRequestableAmount;
                itemContext.ItemStack = itemContext.MaxStack;
            }
            else
            {
                Debug.Log("Count is below request amount");
                itemContext.ItemStack += count;
                break;
            }
        }

        // Call loss manager to direct items that couldn't get a slot. (Don't just forget about the overflow items.)
    }
}

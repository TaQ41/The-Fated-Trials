using System.Collections;
using System.Collections.Generic;
using ItemsCore.Structure;
using UnityEngine;

public struct ItemContext<TItem> where TItem : RuntimeMinimalItem
{
    public string ItemName => Item.ItemName;
    public string ImagePath => Item.ImagePath;

    public TItem Item;
    public int Stack;
}

public class ItemBox3
{
    [SerializeField]
    private List<ItemContext<RuntimeStorageItem>> m_items;

    private (IEnumerable<int> indices, int remainder) PredictItemAppend(RuntimeStorageItem item, int count)
    {
        List<StorageItemContext> contexts = new();

        // Fix:!!! This will be a null reference exception if the object isn't a storageItem.
        int maxStack = item.MaxStack;

        if (maxStack == 1)
        {
            // Find each empty slot and return those.
            return (default, 0);
        }

        for (int i = 0; i < m_items.Count; i++)
        {
            int index = m_items.FindIndex(i, (context) => context.ItemName.Equals(item.ItemName));
            if (index == -1)
                break;
            
            int amountReq = maxStack - m_items[index].Stack;
            
        }

        return default;
    }
}
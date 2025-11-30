using System;
using System.Collections.Generic;
using UnityEngine;
using ItemsCore.Structure;
using Sirenix.OdinInspector;
using Unity.Collections;

[Serializable]
public sealed class ItemBox2<TItem> where TItem : RuntimeMinimalItem
{
    [Serializable]
    public struct ItemContext
    {
        public string ItemName => Item.ItemName;
        public string ItemImagePath => Item.ImagePath;

        public int ItemStack;
        public TItem Item;
    }

    [SerializeField] private List<ItemContext> m_itemContexts;
    [SerializeField] private int m_horizontalBoundSize, m_verticalBoundSize;

    public ItemBox2()
    {
        
    }

    #region Boundaries and Coordinates

    public (int horizontal, int vertical) GetBoundingSizes => (m_horizontalBoundSize, m_verticalBoundSize);
    
    [Button]
    public static Vector2Int GetCoordsByItemPosition(int itemSlot, int horizontalBoundingSize)
    {
        int row = Mathf.FloorToInt(itemSlot / horizontalBoundingSize);
        int col = itemSlot % horizontalBoundingSize;
        return new Vector2Int(col, row);
    }

    [Button]
    public static int GetItemPositionByCoords(Vector2Int coords, int horizontalBoundingSize)
    {
        return coords.y * horizontalBoundingSize + coords.x;
    }

    #endregion

    #region Searching and Indexing

    [Button]
    public bool TryGetItem(Vector2Int coords, out ItemContext context)
    {
        context = default;
        if (coords.x > 0 && coords.x < m_horizontalBoundSize && coords.y > 0 && coords.y < m_verticalBoundSize)
        {
            context = m_itemContexts[GetItemPositionByCoords(coords, m_horizontalBoundSize)];
            return true;
        }

        return false;
    }

    public ItemContext this[Vector2Int coords]
    {
        get
        {
            return m_itemContexts[GetItemPositionByCoords(coords, m_horizontalBoundSize)];
        }

        private set
        {
            m_itemContexts[GetItemPositionByCoords(coords, m_horizontalBoundSize)] = value;
        }
    }

    public int IndexOf_ItemName(TItem item)
    {
        return m_itemContexts.FindIndex((context) => context.ItemName == item.ItemName);
    }

    #endregion

    #region Appending

    public void Append(TItem item, int stack)
    {
        if (item is not RuntimeStorageItem storageItem)
        {
            stack = 1;
        }
        else
        {
            stack = Math.Clamp(stack, 1, storageItem.MaxStack);
        }

        int index = IndexOf_ItemName(item);
    }

    #endregion












    private (IEnumerable<int> indices, int remainder) PredictItemAppend(TItem item, int count)
    {
        List<ItemContext> contexts = new();

        // Fix:!!! This will be a null reference exception if the object isn't a storageItem.
        int maxStack = (item as RuntimeStorageItem).MaxStack;

        if (maxStack == 1)
        {
            // Find each empty slot and return those.
            return (default, 0);
        }

        for (int i = 0; i < m_itemContexts.Count; i++)
        {
            int index = m_itemContexts.FindIndex(i, (context) => context.ItemName.Equals(item.ItemName));
            if (index == -1)
                break;
            
            int amountReq = maxStack - m_itemContexts[index].ItemStack;
            
        }

        return default;
    }




































}

public class ItemBoxTest1 : MonoBehaviour
{
    public ItemBox2<RuntimeStorageItem> ItemBox2;
}
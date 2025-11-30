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
    private List<ItemContext<RuntimeStorageItem>> m_itemBox;

    
}
using System.Collections.Generic;
using ItemsCore.Structure;
using UnityEngine;

namespace InventorySystem
{
    /// <summary>
    /// Use this as a type for the ItemBox when it should be typed as a parent runtime item like RuntimeMinimalItem.
    /// Normally it would not be able to serialize objects that inherit off of the class, which is why this wrapper class is
    /// necessary.
    /// </summary>
    /// <remarks>
    /// Example case would be a reward set that rewards both storage items and pieces of equipment.
    /// Normally you'd have to pass the RMI (RuntimeMinimalItem) class; however, derived classes like
    /// the storage and equipment cannot be serialized due to the uncertainty of the type in a list.
    /// So, this will have to be passed in as the type with the TParentItem as the RMI to serialize
    /// its derived types.
    /// </remarks>
    /// <typeparam name="TParentItem">A parent item type such as the RuntimeMinimalItem.</typeparam>
    public class RuntimeParentItemWrapper<TParentItem>
    {
        [SerializeReference]
        public TParentItem ParentItem;
    }

    [System.Serializable]
    public struct ItemContext<TItem> where TItem : RuntimeMinimalItem
    {
        public int Count;
        public TItem Item;
    }

    /// <summary>
    /// An arbitrary collection used for storing runtime types that represent items or equipment.
    /// Examples of this class's usage would be chests, inventory pouches, or even reward sets.
    /// When needing storage and equipment types, pass in the RuntimeParentItemWrapper and then the RuntimeMinimalItem as
    /// types.
    /// </summary>
    /// <typeparam name="TItem">Type of a runtime item that inherits off of the RuntimeMinimalItem.</typeparam>
    [System.Serializable]
    public class ItemBox<TItem> where TItem : RuntimeMinimalItem
    {
        [SerializeField] private List<TItem> m_items = new();
        [SerializeField] private int HorizontalItemCount, VerticalItemCount;

        // Currently dynamic as the type does not currently exist.
        /// <summary>Sends a message to the manager of this object and the amount of objects to delete. The manager will mutate the list from its methods.</summary>
        [SerializeField] private dynamic ItemLossContextManager;

        public ItemBox(in dynamic itemLossContextManager)
        {
            ItemLossContextManager = itemLossContextManager;
        }

        public (int horizontal, int vertical) GetBoxCoordLengths { get { return (HorizontalItemCount, VerticalItemCount);} }

        /// <summary>
        /// Get items by providing a coords-like set of the index on the horizontal and the index on the vertical to select one item.
        /// XY coordinates (0, 0) is the left-topmost item.
        /// </summary>
        /// <param name="horizontalIndex">How many slots the user has passed horizontally from 0, the origin.</param>
        /// <param name="verticalIndex">How many slots the user has passed vertically from 0, the origin.</param>
        /// <remarks>
        /// The coordinates aren't mapped to a specific slot, but rather a convenience to locate slots based on where the x and y should be.
        /// A horizontal index 10 while the horizontal count is 6 will result in a +4 slot selection starting on the next row from the specified
        /// vertical index.
        /// </remarks>
        /// <returns>
        /// A bool representing if the item could be located or not. 
        /// out: The type specified on this class's construction which inherits from RuntimeMinimalItem.
        /// </returns>
        public bool TryGetItem(int horizontalIndex, int verticalIndex, out TItem item)
        {
            item = null;
            try
            {
                item = m_items[(verticalIndex * VerticalItemCount) + horizontalIndex];
                return true;
            }
            catch (System.IndexOutOfRangeException)
            {
                Debug.LogError($"The index x:{horizontalIndex} y:{verticalIndex} which on bounds x:{HorizontalItemCount} y:{VerticalItemCount} at slot: "
                + $"{(verticalIndex * VerticalItemCount) + horizontalIndex} was below or above the expected ItemBox's range "
                + $"(0 to {(VerticalItemCount * HorizontalItemCount) - 1}). The actual range of the ItemBox is {m_items.Count}");
                return false;
            }
            catch (System.NullReferenceException err)
            {
                Debug.LogError($"The item list has not been initialized." + err.Message);
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// View the method 'TryGetItem' for a summary and remarks.
        /// Different that it does not guarantee protection against IndexOutOfRangeExceptions.
        /// This can also set the item slot to a value of TItem.
        /// </summary>
        /// <param name="horizontalIndex">How many slots the user has passed horizontally from 0, the origin.</param>
        /// <param name="verticalIndex">How many slots the user has passed vertically from 0, the origin.</param>
        /// <returns>The type specified on this class's construction which inherits from RuntimeMinimalItem.</returns>
        public TItem this[int horizontalIndex, int verticalIndex]
        {
            get
            {
                return m_items[(verticalIndex * HorizontalItemCount) + horizontalIndex];
            }

            set
            {
                m_items[(verticalIndex * HorizontalItemCount) + horizontalIndex] = value;
            }
        }

        // Allow stacking and handle overflow.

        public void Append(TItem item)
        {
            // Requires a count context of the selected item.
        }

        public void Append(IEnumerable<TItem> items)
        {
            
        }
    }
}
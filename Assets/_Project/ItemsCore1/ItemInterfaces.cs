using UnityEngine;

namespace ItemsCore
{
    public interface IItem
    {
        public abstract string ItemDisplayName { get; }
        public abstract string ItemName { get; }
    }

    #region Equipment Item Interfaces

    public interface IEquipmentItem : IItem
    {
        public abstract int Attack { get; }
        public abstract int Defense { get; }
    }

    #endregion

    #region Storage Item Interfaces

    public interface IStorageItem : IItem
    {
        public abstract int MaxStack { get; }
    }

    public interface IConsumableItem : IStorageItem
    {
        public abstract void Consume();
    }

    public interface IPlaceableItem : IStorageItem
    {
        public abstract void Place();
    }

    #endregion
}
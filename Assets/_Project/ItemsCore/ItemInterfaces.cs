namespace ItemsCore.Structure
{
    public interface IItem
    {
        public abstract string ItemName { get; }
    }

    public interface IEquipmentItem : IItem
    {
        public abstract int Attack { get; }
        public abstract int Defense { get; }
    }

    public interface IStorageItem : IItem
    {
        public abstract int MaxCount { get; }
        public string ItemType { get; }
    }
}

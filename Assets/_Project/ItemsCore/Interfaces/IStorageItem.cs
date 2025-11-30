namespace ItemsCore.Structure
{
    public interface IStorageItem : IItem
    {
        public abstract int MaxStack { get; }
        public string ItemType { get; }
    }
}
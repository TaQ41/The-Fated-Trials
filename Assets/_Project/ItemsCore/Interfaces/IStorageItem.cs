namespace ItemsCore.Structure
{
    public interface IStorageItem : IItem
    {
        public abstract int MaxCount { get; }
        public string ItemType { get; }
    }
}
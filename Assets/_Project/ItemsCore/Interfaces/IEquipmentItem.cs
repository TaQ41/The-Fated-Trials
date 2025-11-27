namespace ItemsCore.Structure
{
    public interface IEquipmentItem : IItem
    {
        public abstract int Attack { get; }
        public abstract int Defense { get; }
    }
}
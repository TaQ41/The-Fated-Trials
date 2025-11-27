namespace ItemsCore.Structure
{
    [System.Serializable]
    public class RuntimeEquipmentItem : RuntimeMinimalItem, IEquipmentItem
    {
        public int Attack { get; }
        public int Defense { get; }

        public RuntimeEquipmentItem(string _itemName, int _attack, int _defense,
            string _imagePath = "") : base(_itemName, _imagePath)
        {
            Attack = _attack;
            Defense = _defense;
        }
    }
}

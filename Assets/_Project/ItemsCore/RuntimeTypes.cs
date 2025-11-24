namespace ItemsCore.Structure
{
    public class RuntimeMinimalItem : IItem
    {
        public string ItemName { get; }
        public string ImagePath { get; }

        public RuntimeMinimalItem(string _itemName, string _imagePath = "")
        {
            ItemName = _itemName;
            ImagePath = _imagePath;
            if (_imagePath == string.Empty)
            {
                // Run lookup on ItemName and get imagePath.
            }
        }
    }

    public class RuntimeStorageItem : RuntimeMinimalItem, IStorageItem
    {
        public int MaxCount { get; }
        public string ItemType { get; }

        public RuntimeStorageItem(string _itemName, int _maxCount, string _itemType,
            string _imagePath = "") : base(_itemName, _imagePath)
        {
            MaxCount = _maxCount;
            ItemType = _itemType;
        }
    }

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
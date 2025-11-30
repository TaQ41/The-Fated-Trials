using UnityEngine;

namespace ItemsCore.Structure
{
    [System.Serializable]
    public class RuntimeEquipmentItem : RuntimeMinimalItem, IEquipmentItem
    {
        [SerializeField] private int m_attack;
        [SerializeField] private int m_defense;
        
        public int Attack => m_attack;
        public int Defense => m_defense;

        public RuntimeEquipmentItem(string _itemName, int _attack, int _defense,
            string _imagePath = "") : base(_itemName, _imagePath)
        {
            m_attack = _attack;
            m_defense = _defense;
        }
    }
}

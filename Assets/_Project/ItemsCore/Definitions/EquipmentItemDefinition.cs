using UnityEngine;

namespace ItemsCore.Structure
{
    [CreateAssetMenu(fileName = "EquipmentItemDefinition", menuName = "Items/EquipmentItemDefinition")]
    public class EquipmentItemDefinition : ItemDefinition, IEquipmentItem
    {
        [SerializeField] private int m_attack;
        [SerializeField] private int m_defense;

        public int Attack => m_attack;
        public int Defense => m_defense;

        public override IItem CreateRuntimeItem()
        {
            return new RuntimeEquipmentItem(_itemName: m_itemName, _attack: m_attack, _defense: m_defense,
                _imagePath: m_imagePath);
        }
    }
}
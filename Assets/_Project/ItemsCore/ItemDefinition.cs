using Sirenix.OdinInspector;
using UnityEngine;

namespace ItemsCore.Structure
{   
    [CreateAssetMenu(fileName = "ItemDefinition", menuName = "Items/ItemDefinition")]
    public class ItemDefinition : SerializedScriptableObject, IItem
    {
        [SerializeField] protected string m_itemName;
        [SerializeField] protected string m_imagePath;

        public string ItemName => m_itemName;
        public string ImagePath => m_imagePath;

        public virtual IItem CreateRuntimeItem()
        {
            return new RuntimeMinimalItem(m_itemName, m_imagePath);
        }

        public IEquipmentItem AsEquipmentDef()
        {
            return CreateRuntimeItem() as IEquipmentItem;
        }

        public IStorageItem AsStorageDef()
        {
            return CreateRuntimeItem() as IStorageItem;
        }
    }
}
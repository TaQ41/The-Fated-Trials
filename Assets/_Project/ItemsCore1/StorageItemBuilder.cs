using UnityEngine;

namespace ItemsCore
{
    [CreateAssetMenu(fileName = "StorageItemBuilder", menuName = "Scriptable Objects/StorageItemBuilder")]
    public class StorageItemBuilder : ScriptableObject, IStorageItem
    {
        [SerializeField] private string m_itemDisplayName;
        [SerializeField] private string m_itemName;
        [SerializeField] private int m_maxStack;

        public string ItemDisplayName => m_itemDisplayName;
        public string ItemName => m_itemName;
        public int MaxStack => m_maxStack;

        [SerializeField] private ItemToFileTranslator.ItemTypes m_interfaceType;

        private System.Collections.Generic.List<string> GetAdditionalStorageFieldsText()
        {
            return new System.Collections.Generic.List<string>()
            {
                $"        public int {nameof(MaxStack)} {{ get {{ return {m_maxStack};}} }}"
            };
        }

        [Sirenix.OdinInspector.Button]
        public void BuildBoilerPlateStorageItem(string fileName)
        {
            ItemToFileTranslator.GenerateBoilerPlateFileCode<IStorageItem>(
                fileName,
                item: this, 
                additionalMembers: GetAdditionalStorageFieldsText(),
                interfaceType: m_interfaceType);
        }
    }
}

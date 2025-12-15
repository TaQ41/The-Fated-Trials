using ItemsCore.Structure;
using UnityEngine;

public class StorageItemBoxTest : MonoBehaviour
{
    public StorageItemBox storageItemBox;
    [SerializeField] private ItemLookup m_itemLookup;

    [Sirenix.OdinInspector.Button]
    public void DoAppend(RuntimeStorageItem def, int count)
    {
        storageItemBox.Append(def, count);
    }

    [Sirenix.OdinInspector.Button]
    public RuntimeStorageItem CreateRuntime(string itemName)
    {
        var (item, success) = m_itemLookup.Lookup(itemName);
        if (success)
        {
            return item.CreateRuntimeItem() as RuntimeStorageItem;
        }

        return default;
    }
}

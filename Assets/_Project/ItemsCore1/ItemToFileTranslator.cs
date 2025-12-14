using System.IO;
using System.Collections.Generic;
using System;

namespace ItemsCore
{
    internal static class ItemToFileTranslator
    {
        internal enum ItemTypes
        {
            IStorageItem = 0,
            IConsumableItem = 1,
            IPlaceableItem = 2
        }

        private static (string, List<string>) GetInterfaceTypeMethodLines(ItemTypes interfaceType)
        {
            return (Enum.GetName(typeof(ItemTypes), interfaceType),
                    interfaceType switch
            {
                ItemTypes.IConsumableItem => new List<string>()
                {
                    "        public void Consume()",
                    "        {",
                    "            throw new System.NotImplementedException();",
                    "        }",
                },
                ItemTypes.IPlaceableItem => new List<string>()
                {
                    "        public void Place()",
                    "        {",
                    "            throw new System.NotImplementedException();",
                    "        }",
                },
                _ => new List<string>(),
            });
        }

        static readonly string ItemFilesDirectoryPath = "Assets\\_Project\\ItemsCore1\\Items";

        public static void GenerateBoilerPlateFileCode<TItem>(string fileName, TItem item, List<string> additionalMembers, ItemTypes interfaceType) where TItem : IItem
        {
            (string interfaceTypeText, List<string> interfaceMethods) = GetInterfaceTypeMethodLines(interfaceType);
            List<string> fileText = new()
            {
                "",
                "",
                "namespace " + nameof(ItemsCore) + ".Items",
                "{",
               $"    public class {fileName} : {interfaceTypeText}",
                "    {",
               $"        public string {nameof(item.ItemName)} {{ get {{ return \"{item.ItemName}\";}} }}",
               $"        public string {nameof(item.ItemDisplayName)} {{ get {{ return \"{item.ItemDisplayName}\";}} }}",
            };

            fileText.AddRange(additionalMembers);
            fileText.AddRange(interfaceMethods);
            fileText.AddRange(new List<string>()
            {
                "    }",
                "}"
            });

            File.WriteAllText(ItemFilesDirectoryPath + "\\" + fileName + ".cs", string.Join('\n', fileText));
        }
    }
}
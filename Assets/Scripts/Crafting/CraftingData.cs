using System.Collections.Generic;

namespace Crafting {

    public abstract class CraftingData : CraftingStation {

        public static Dictionary<string, CraftingData> nameToData = new Dictionary<string, CraftingData>();

        public static CraftingData WORKBENCH = new WorkBench("Workbench");

        public CraftingData(string name){
            nameToData[name] = this;
        }

    }

}
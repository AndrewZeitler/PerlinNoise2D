using Entities;
using UI;
using Crafting;

namespace Tiles {

    public class CraftingStation : TileModifier {

        public string name;
        string dataName;
        public CraftingData craftingData { get; private set; }
        public Inventory input { get; private set; }
        public Inventory output { get; private set; }


        public CraftingStation(string name, string dataName, Inventory input, Inventory output) {
            this.name = name;
            this.dataName = dataName;
            this.input = input;
            this.output = output;
        }

        public override void Initialize(Tile tile){
            craftingData = CraftingData.nameToData[dataName];
            this.tile = tile;
            tile.AddInteractListener(OnInteract);
        }

        public void OnInteract(Entity entity){
            Player player = entity as Player;
            if(player != null){
                PageUI pageUI = MenuManager.CreatePage(name);
                WorkBenchUI workBenchUI = new WorkBenchUI();
                workBenchUI.CreateUI(craftingData, input, output, pageUI);
                if(!MenuManager.IsMenuActive()) MenuManager.ToggleMenu();
                MenuManager.SetTab(MenuManager.GetPagesCount() - 1);
            }
        }

    }

}
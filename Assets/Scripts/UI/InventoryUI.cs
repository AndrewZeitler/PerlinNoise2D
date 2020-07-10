using UnityEngine;
using UI.Elements;

namespace UI {
    public class InventoryUI {
        Inventory inventory;
        public void CreateUI(Inventory inventory, PageUI page){
            this.inventory = inventory;

            for(int i = 0; i < inventory.GetInventorySize(); ++i){
                OpenSlotUI slot = new OpenSlotUI(new Vector2(i % 4f * OpenSlotUI.SLOT_SIZE.x, i / 4 * OpenSlotUI.SLOT_SIZE.y), i, inventory);
                slot.SetParent(page.content.transform);
            }
        }
    }
}

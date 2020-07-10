using UnityEngine;
using UI.Elements;
using Entities;

namespace UI {
    public class PlayerInventoryUI
    {
        Inventory inventory;
        public void CreateUI(Player player, PageUI page){
            this.inventory = player.inventory;
            for(int i = 0; i < Player.INVENTORY_SIZE; ++i){
                OpenSlotUI slot = new OpenSlotUI(new Vector2(i % 4f * OpenSlotUI.SLOT_SIZE.x, 1f - i / 4 * OpenSlotUI.SLOT_SIZE.y), i + Player.HOTBAR_SIZE, player.inventory);
                slot.SetParent(page.content.transform);
            }
            //page.AddInventory(Vector2.zero, inventory, new Utils.RangeInt(Player.HOTBAR_SIZE, inventory.GetInventorySize()));
        }
    }
}

using UnityEngine;
using Items;

namespace Entities {

    public class Player : Entity {
        PlayerController controller;
        public Hotbar hotbar { get; private set; }

        public Player(string name, Vector2 pos) : base(name) {
            GameObject prefab = Resources.Load("Prefabs/" + name) as GameObject;
            inventory = new Inventory(28, 4, "Inventory");
            inventory.AddItem(new ItemStack(ItemData.PICKAXE, 10));
            inventory.AddItem(new ItemStack(ItemData.SHOVEL, 1));
            inventory.AddItem(new ItemStack(ItemData.WOOD, 10));
            hotbar = new Hotbar(this);
            entityObject = PlayerController.Instantiate(prefab, pos, Quaternion.identity);
        }

        public override bool AddItem(ItemStack itemStack){
            if(!hotbar.inventory.AddItem(itemStack)){
                if(!inventory.AddItem(itemStack)){
                    return false;
                }
            }
            return true;
        }

        public ItemStack GetHeldItem(){
            return hotbar.inventory.GetItemStackAt(hotbar.selectedSlot);
        }

    }

}
using UnityEngine;

namespace Entities {

    public abstract class Entity {
        public string name { get; }
        public GameObject entityObject { get; protected set; }
        public Inventory inventory { get; protected set; }

        public Entity(string name){
            this.name = name;
        }

        public virtual bool AddItem(ItemStack itemStack) {
            return inventory.AddItem(itemStack);
        }

    }

}

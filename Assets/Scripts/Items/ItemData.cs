using System;

namespace Items {

    public class ItemData : ItemEnum {
        public static readonly ItemData WOOD = new ItemData("Wood", 1, new ItemModifier[]{});
        public static readonly ItemData PICKAXE = new ItemData("Pickaxe", 2, new ItemModifier[]{});
        public static readonly ItemData SHOVEL = new ItemData("Shovel", 3, new ItemModifier[]{});

        public ItemData(string name, int id, ItemModifier[] modifiers) : base(name, id, modifiers) {}

        public ItemModifier GetModifierOfType<T>(){
            foreach(ItemModifier modifier in ItemModifiers){
                if(modifier.GetType() == typeof(T)) return modifier;
            }
            return null;
        }
    }
    
}
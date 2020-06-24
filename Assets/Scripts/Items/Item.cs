using UnityEngine;
using UnityEngine.Events;
using Events;
using Entities;

namespace Items {

    public class Item {
        public ItemData itemData;
        public GameObject gameObject;

        ItemModifier[] modifiers;

        EntityEvent OnLeftClick;
        EntityEvent OnRightClick;

        public Item(ItemData itemData){
            this.itemData = itemData;

            OnLeftClick = new EntityEvent();
            OnRightClick = new EntityEvent();
        }

        public void InitializeModifiers(){
            modifiers = new ItemModifier[itemData.ItemModifiers.Length];
            for(int i = 0; i < modifiers.Length; ++i){
                modifiers[i] = itemData.ItemModifiers[i].Clone();
                modifiers[i].Intiailize(this);
            }
        }

        public void AddLeftClickListener(UnityAction<Entity> listener){
            OnLeftClick.AddListener(listener);
        }

        public void AddRightClickListener(UnityAction<Entity> listener){
            OnRightClick.AddListener(listener);
        }

        public void OnLeftClickEvent(Entity entity){
            OnLeftClick.Invoke(entity);
        }

        public void OnRightClickEvent(Entity entity){
            OnRightClick.Invoke(entity);
        }

        public ItemModifier GetModifierOfType<T>(){
            foreach(ItemModifier modifier in modifiers){
                if(modifier.GetType() == typeof(T)) return modifier;
            }
            return null;
        }
    }
    
}
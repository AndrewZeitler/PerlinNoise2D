using UnityEngine;
using UnityEngine.Events;
using Events;
using Entities;

namespace Items {

    public class Item {
        public ItemData itemData;
        public GameObject gameObject;

        ItemModifier[] modifiers;

        ItemUseEvent OnLeftClick;
        ItemUseEvent OnRightClick;
        ItemUseEvent ReleaseLeftClick;
        ItemUseEvent ReleaseRightClick;

        public Item(ItemData itemData){
            this.itemData = itemData;

            OnLeftClick = new ItemUseEvent();
            OnRightClick = new ItemUseEvent();
            ReleaseLeftClick = new ItemUseEvent();
            ReleaseRightClick = new ItemUseEvent();

            InitializeModifiers();
        }

        public void InitializeModifiers(){
            modifiers = new ItemModifier[itemData.ItemModifiers.Length];
            for(int i = 0; i < modifiers.Length; ++i){
                modifiers[i] = itemData.ItemModifiers[i].Clone();
                modifiers[i].Initialize(this);
            }
        }

        public void AddLeftClickListener(UnityAction<Entity, Vector3> listener){
            OnLeftClick.AddListener(listener);
        }

        public void AddRightClickListener(UnityAction<Entity, Vector3> listener){
            OnRightClick.AddListener(listener);
        }

        public void AddReleaseLeftClickListener(UnityAction<Entity, Vector3> listener){
            ReleaseLeftClick.AddListener(listener);
        }

        public void AddReleaseRightClickListener(UnityAction<Entity, Vector3> listener){
            ReleaseRightClick.AddListener(listener);
        }

        public void OnLeftClickEvent(Entity entity, Vector2 pos){
            OnLeftClick.Invoke(entity, pos);
        }

        public void OnRightClickEvent(Entity entity, Vector2 pos){
            OnRightClick.Invoke(entity, pos);
        }

        public void ReleaseLeftClickEvent(Entity entity, Vector2 pos){
            ReleaseLeftClick.Invoke(entity, pos);
        }

        public void ReleaseRightClickEvent(Entity entity, Vector2 pos){
            ReleaseRightClick.Invoke(entity, pos);
        }

        public ItemModifier GetModifierOfType<T>(){
            foreach(ItemModifier modifier in modifiers){
                if(modifier.GetType() == typeof(T)) return modifier;
            }
            return null;
        }
    }
    
}
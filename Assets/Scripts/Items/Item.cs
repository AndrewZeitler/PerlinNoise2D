using UnityEngine;
using UnityEngine.Events;
using Events;

namespace Items {

    public class Item {
        public ItemData itemData;
        public Sprite sprite;
        public GameObject gameObject;

        ItemModifier[] modifiers;

        PlayerEvent OnLeftClick;
        PlayerEvent OnRightClick;

        public Item(ItemData itemData){
            this.itemData = itemData;

            OnLeftClick = new PlayerEvent();
            OnRightClick = new PlayerEvent();

            sprite = SpriteManager.GetItemSprite(itemData.Name);
        }

        public void InitializeModifiers(){
            modifiers = new ItemModifier[itemData.ItemModifiers.Length];
            for(int i = 0; i < modifiers.Length; ++i){
                modifiers[i] = itemData.ItemModifiers[i].Clone();
                modifiers[i].Intiailize(this);
            }
        }

        public void AddLeftClickListener(UnityAction<PlayerController> listener){
            OnLeftClick.AddListener(listener);
        }

        public void AddRightClickListener(UnityAction<PlayerController> listener){
            OnRightClick.AddListener(listener);
        }

        public void OnLeftClickEvent(PlayerController player){
            OnLeftClick.Invoke(player);
        }

        public void OnRightClickEvent(PlayerController player){
            OnRightClick.Invoke(player);
        }
    }
    
}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UI.Elements {

    public abstract class SlotUI {
        static GameObject slotPrefab;
        public GameObject gameObject { get; protected set; }
        public RectTransform rectTransform { get; protected set; }
        public ItemSlot itemSlot { get; protected set; }

        public virtual void SlotChanged(){
            ItemSlot slot = gameObject.GetComponent<ItemSlot>();
            if(slot == null) return;
            ItemStack item = slot.inventory.GetItemStackAt(slot.index);
            Image image = slot.transform.GetChild(0).GetComponent<Image>();
            Text text = slot.transform.GetChild(1).GetComponent<Text>();
            slot.itemStack = item;
            if(item != null){
                image.sprite = item.GetItem().itemData.ItemSprite;
                image.color = new Color(1, 1, 1, 1);
                if(item.GetAmount() > 1){
                    text.text = item.GetAmount().ToString();
                } else {
                    text.text = "";
                }
            } else {
                image.sprite = null;
                image.color = new Color(1, 1, 1, 0);
                text.text = "";
            }
        }

        public virtual void SetParent(Transform parent){
            Vector2 min = rectTransform.offsetMin; 
            Vector2 max = rectTransform.offsetMax;
            gameObject.transform.SetParent(parent);
            rectTransform.offsetMin = min;
            rectTransform.offsetMax = max;
        }

        public abstract void ItemSlotClick(PointerEventData pointer, ItemSlot clickedSlot);

        public abstract void ItemSlotEnter(PointerEventData pointer, ItemSlot itemSlot);

        public abstract void ItemSlotExit(PointerEventData pointer, ItemSlot itemSlot);

    }

}
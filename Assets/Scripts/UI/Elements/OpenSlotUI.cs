using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Elements {

    public class OpenSlotUI : SlotUI {
        public static Vector2 SLOT_SIZE { get; private set; } = new Vector2(0.25f, (Screen.width * 0.07125f) / Screen.height);
        static GameObject slotPrefab;

        public OpenSlotUI(Vector2 pos, int index, Inventory inventory){
            if(slotPrefab == null) slotPrefab = Resources.Load("Prefabs/ItemSlot") as GameObject;
            gameObject = GameObject.Instantiate(slotPrefab, Vector3.zero, Quaternion.identity);
            rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = pos;
            rectTransform.anchorMax = pos + SLOT_SIZE;
            itemSlot = gameObject.GetComponent<ItemSlot>();
            itemSlot.index = index;
            itemSlot.inventory = inventory;
            itemSlot.OnClick = ItemSlotClick;
            itemSlot.OnEnter = ItemSlotEnter;
            itemSlot.OnExit = ItemSlotExit;
        }

        public override void ItemSlotClick(PointerEventData pointer, ItemSlot clickedSlot){
            GameObject heldItem = MenuManager.heldItem;
            ItemStack item = null;
            if(heldItem != null){
                item = heldItem.GetComponent<HeldItem>().itemStack;
            }
            int slotIndex = clickedSlot.index;
            MenuManager.ItemSlotClick(pointer, clickedSlot, clickedSlot.inventory.AddItemAt(slotIndex, item));
            SlotChanged();
        }

        public override void ItemSlotEnter(PointerEventData pointer, ItemSlot itemSlot){
            MenuManager.ItemSlotEnter(pointer, itemSlot);
        }

        public override void ItemSlotExit(PointerEventData pointer, ItemSlot itemSlot){
            MenuManager.ItemSlotExit(pointer, itemSlot);
        }

    }

}
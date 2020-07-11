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
            ItemStack item = null;
            if(MenuManager.itemHeld != null){
                item = MenuManager.itemHeld.itemStack;
            }
            int slotIndex = clickedSlot.index;

            ItemStack newItem = clickedSlot.inventory.AddItemAt(slotIndex, item);
            MenuManager.SetHeldItem(newItem);
            MenuManager.SetDescriptor(new ItemDescriptorUI(item));
            SlotChanged();
        }

        public override void ItemSlotEnter(PointerEventData pointer, ItemSlot itemSlot){
            MenuManager.SetDescriptor(new ItemDescriptorUI(itemSlot.itemStack));
        }

        public override void ItemSlotExit(PointerEventData pointer, ItemSlot itemSlot){
            if(MenuManager.descriptor != null) {
                MenuManager.descriptor.DestroyUI();
                MenuManager.descriptor = null;
            }
        }

    }

}
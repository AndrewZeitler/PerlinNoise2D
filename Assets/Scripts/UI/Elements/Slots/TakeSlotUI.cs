using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Elements {

    public class TakeSlotUI : SlotUI {
        static GameObject slotPrefab;

        public TakeSlotUI(int index, Inventory inventory){
            if(slotPrefab == null) slotPrefab = Resources.Load("Prefabs/ItemSlot") as GameObject;
            gameObject = GameObject.Instantiate(slotPrefab, Vector3.zero, Quaternion.identity);
            rectTransform = gameObject.GetComponent<RectTransform>();
            itemSlot = gameObject.GetComponent<ItemSlot>();
            itemSlot.index = index;
            itemSlot.inventory = inventory;
            itemSlot.OnClick = ItemSlotClick;
            itemSlot.OnEnter = ItemSlotEnter;
            itemSlot.OnExit = ItemSlotExit;
            inventory.AddListener(SlotChanged);
        }

        public override void ItemSlotClick(PointerEventData pointer, ItemSlot clickedSlot){
            if(MenuManager.itemHeld != null) return;
            int slotIndex = clickedSlot.index;

            ItemStack newItem = clickedSlot.inventory.AddItemAt(slotIndex, null);
            MenuManager.SetHeldItem(newItem);
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
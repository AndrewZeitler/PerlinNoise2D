using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Elements {

    public class HotbarSlotUI : SlotUI {
        public static Vector2 SLOT_SIZE { get; private set; } = new Vector2(0.1f / (Screen.width / (float)Screen.height), 0.1f);
        static GameObject slotPrefab;
        Entities.Player player;

        public HotbarSlotUI(Vector2 pos, int index, Entities.Player player){
            if(slotPrefab == null) slotPrefab = Resources.Load("Prefabs/ItemSlot") as GameObject;
            this.player = player;
            gameObject = GameObject.Instantiate(slotPrefab, Vector3.zero, Quaternion.identity);
            rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = pos;
            rectTransform.anchorMax = pos + SLOT_SIZE;
            itemSlot = gameObject.GetComponent<ItemSlot>();
            itemSlot.index = index;
            itemSlot.inventory = player.inventory;
            itemSlot.OnClick = ItemSlotClick;
            itemSlot.OnEnter = ItemSlotEnter;
            itemSlot.OnExit = ItemSlotExit;
        }

        public override void ItemSlotClick(PointerEventData pointer, ItemSlot clickedSlot){
            if(MenuManager.IsMenuActive()){
                ItemStack item = null;
                if(MenuManager.itemHeld != null){
                    item = MenuManager.itemHeld.itemStack;
                }
                int slotIndex = clickedSlot.index;

                ItemStack newItem = clickedSlot.inventory.AddItemAt(slotIndex, item);
                MenuManager.SetHeldItem(newItem);
                MenuManager.SetDescriptor(new ItemDescriptorUI(item));

                SlotChanged();
            } else {
                player.SetSelectedSlot(clickedSlot.index);
            }
        }

        public override void ItemSlotEnter(PointerEventData pointer, ItemSlot itemSlot){
            if(MenuManager.IsMenuActive()){
                MenuManager.SetDescriptor(new ItemDescriptorUI(itemSlot.itemStack));
            }
        }

        public override void ItemSlotExit(PointerEventData pointer, ItemSlot itemSlot){
            if(MenuManager.IsMenuActive()){
                if(MenuManager.descriptor != null) {
                    MenuManager.descriptor.DestroyUI();
                    MenuManager.descriptor = null;
                }
            }
        }

    }

}
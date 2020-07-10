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
                GameObject heldItem = MenuManager.heldItem;
                ItemStack item = null;
                if(heldItem != null){
                    item = heldItem.GetComponent<HeldItem>().itemStack;
                }
                int slotIndex = clickedSlot.index;
                MenuManager.ItemSlotClick(pointer, clickedSlot, clickedSlot.inventory.AddItemAt(slotIndex, item));
                SlotChanged();
            } else {
                player.SetSelectedSlot(clickedSlot.index);
            }
        }

        public override void ItemSlotEnter(PointerEventData pointer, ItemSlot itemSlot){
            if(MenuManager.IsMenuActive()){
                MenuManager.ItemSlotEnter(pointer, itemSlot);
            }
        }

        public override void ItemSlotExit(PointerEventData pointer, ItemSlot itemSlot){
            if(MenuManager.IsMenuActive()){
                MenuManager.ItemSlotExit(pointer, itemSlot);
            }
        }

    }

}
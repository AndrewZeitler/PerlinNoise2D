using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Elements {

    public class ClosedSlotUI : SlotUI {
        static GameObject slotPrefab;

        public ClosedSlotUI(int index, Inventory inventory){
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

        public override void ItemSlotClick(PointerEventData pointer, ItemSlot clickedSlot){}

        public override void ItemSlotEnter(PointerEventData pointer, ItemSlot itemSlot){}

        public override void ItemSlotExit(PointerEventData pointer, ItemSlot itemSlot){}

    }

}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.EventSystems;
//using Utils;

namespace UI {

    public class PageUI {

        public static GameObject pagePrefab;
        public static GameObject slotPrefab;
        public GameObject page { get; private set; }
        public GameObject content { get; private set; }

        public List<Inventory> inventories = new List<Inventory>();

        public PageUI(string name){
            if(pagePrefab == null) {
                pagePrefab = Resources.Load("Prefabs/Page") as GameObject;
            }
            page = GameObject.Instantiate(pagePrefab);
            page.transform.SetParent(MenuManager.menu.transform);

            GameObject tab = page.transform.GetChild(0).gameObject;
            RectTransform tabTransform = tab.GetComponent<RectTransform>();
            tabTransform.anchorMin = new Vector2(tabTransform.anchorMin.x, 0.9f - (MenuManager.GetPagesCount() / 10f));
            tabTransform.anchorMax = new Vector2(tabTransform.anchorMax.x, 1f - (MenuManager.GetPagesCount() / 10f));

            Button button = tab.GetComponent<Button>();
            button.GetComponentInChildren<Text>().text = name;
            button.onClick.AddListener(delegate { OnTabClick(page); });

            RectTransform pageTransform = page.GetComponent<RectTransform>();
            pageTransform.offsetMin = pageTransform.offsetMax = Vector3.zero;

            GameObject panel = page.transform.GetChild(1).gameObject;

            GameObject scroll = new GameObject("Scroll");
            RectTransform scrollRect = scroll.AddComponent<RectTransform>();
            scrollRect.anchorMin = Vector2.zero;
            scrollRect.anchorMax = Vector2.one;
            scroll.AddComponent<ScrollRect>();
            scroll.transform.SetParent(panel.transform);

            content = new GameObject("Content");
            content.AddComponent<RectTransform>();
            RectTransform contentTransform = content.GetComponent<RectTransform>();
            contentTransform.anchorMin = Vector2.zero;
            contentTransform.anchorMax = Vector2.one;
            content.transform.SetParent(scroll.transform);

            scroll.GetComponent<ScrollRect>().content = contentTransform;
            scroll.GetComponent<ScrollRect>().scrollSensitivity = 50;
            RectTransform scrollTransform = scroll.GetComponent<RectTransform>();
            scrollTransform.offsetMin = scrollTransform.offsetMax = Vector3.zero;
        }

        // public void AddInventory(Vector2 position, Inventory inventory, Utils.RangeInt bounds = null){
        //     if(bounds == null) bounds = new Utils.RangeInt(0, inventory.GetInventorySize());
        //     if(slotPrefab == null) {
        //         slotPrefab = Resources.Load("Prefabs/ItemSlot") as GameObject;
        //     }
        //     inventories.Add(inventory);

        //     float aspectRatio = Screen.width / (float)Screen.height;
        //     float slotHeight = (1f / inventory.GetColumns() * 0.3f * aspectRatio);
        //     RectTransform uiTransform = content.GetComponent<RectTransform>();
        //     uiTransform.anchorMax = new Vector2(1, slotHeight * ((bounds.max - bounds.min) / inventory.GetColumns()));

        //     for(int i = bounds.min; i < bounds.max; ++i){
        //         GameObject slot = GameObject.Instantiate(slotPrefab);
        //         slot.transform.SetParent(content.transform);
        //         ItemSlot itemSlot = slot.AddComponent<ItemSlot>();
        //         itemSlot.index = i;
        //         itemSlot.inventory = inventory;
        //         RectTransform slotTransform = slot.GetComponent<RectTransform>();
        //         slotTransform.offsetMin = slotTransform.offsetMax = Vector3.zero;
        //         slotTransform.anchorMin = new Vector2(1f / inventory.GetColumns() * ((i - bounds.min) % inventory.GetColumns()), 
        //                                                 1f - slotHeight * ((i - bounds.min) / inventory.GetColumns() + 1) / uiTransform.anchorMax.y);
        //         slotTransform.anchorMax = new Vector2(1f / inventory.GetColumns() * ((i - bounds.min) % inventory.GetColumns() + 1), 
        //                                                 1f - slotHeight * ((i - bounds.min) / inventory.GetColumns()) / uiTransform.anchorMax.y);
        //     }
        //     SetOnClick(inventory, ItemSlotClick);
        //     SetOnEnter(inventory, ItemSlotEnter);
        //     SetOnExit(inventory, ItemSlotExit);
        //     InventoryChanged();
        //     inventory.AddListener(InventoryChanged);
        // }

        // public void SetOnClick(Inventory inventory, UnityAction<PointerEventData, ItemSlot> OnClick){
        //     Debug.Log(content.transform.childCount);
        //     for(int i = 0; i < content.transform.childCount; ++i){
        //         ItemSlot itemSlot = content.transform.GetChild(i).GetComponent<ItemSlot>();
        //         if(itemSlot == null) continue;
        //         if(itemSlot.inventory == inventory) {
        //             itemSlot.OnClick = OnClick;
        //         }
        //     }
        // }

        // public void SetOnEnter(Inventory inventory, UnityAction<PointerEventData, ItemSlot> OnEnter){
        //     for(int i = 0; i < content.transform.childCount; ++i){
        //         ItemSlot itemSlot = content.transform.GetChild(i).GetComponent<ItemSlot>();
        //         if(itemSlot == null) continue;
        //         if(itemSlot.inventory == inventory) {
        //             itemSlot.OnEnter = OnEnter;
        //         }
        //     }
        // }

        // public void SetOnExit(Inventory inventory, UnityAction<PointerEventData, ItemSlot> OnExit){
        //     for(int i = 0; i < content.transform.childCount; ++i){
        //         ItemSlot itemSlot = content.transform.GetChild(i).GetComponent<ItemSlot>();
        //         if(itemSlot == null) continue;
        //         if(itemSlot.inventory == inventory) {
        //             itemSlot.OnExit = OnExit;
        //         }
        //     }
        // }

        public void PageChanged(){
            for(int i = 0; i < content.transform.childCount; ++i){
                ItemSlot slot = content.transform.GetChild(i).GetComponent<ItemSlot>();
                if(slot == null) continue;
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
        }

        public void OnTabClick(GameObject click){
            MenuManager.OnTabClick(click);
        }
    }

}
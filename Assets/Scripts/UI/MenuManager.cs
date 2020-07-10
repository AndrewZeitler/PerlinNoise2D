using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Entities;

namespace UI {

    public class MenuManager : MonoBehaviour
    {   
        public GameObject serializedMenu;
        
        public static GameObject menu;
        public static GameObject slotPrefab;
        public static GameObject descriptionPrefab;

        static List<PageUI> pages = new List<PageUI>();

        private static MenuManager menuManager;
        private static GameObject emptyPrefab;

        private static int currentTab = 0;
        public static GameObject heldItem { get; private set; } = null;
        private static GameObject itemDescription = null;
        private static UnityEvent inventoryOpen;
        private static HotbarUI hotbar;

        void Start() {
            MenuManager.menu = serializedMenu;
            MenuManager.slotPrefab = Resources.Load("Prefabs/ItemSlot") as GameObject;
            MenuManager.descriptionPrefab = Resources.Load("Prefabs/ItemDescriptor") as GameObject;
            menuManager = this;
            emptyPrefab = new GameObject();
            RectTransform emptyRect = emptyPrefab.AddComponent<RectTransform>();
            emptyRect.anchorMin = Vector2.zero;
            emptyRect.anchorMax = Vector2.one;
            inventoryOpen = new UnityEvent();
        }

        public static int GetPagesCount(){
            return pages.Count;
        }

        public static void ToggleMenu(){
            menu.SetActive(!menu.activeSelf);
            if(menu.activeSelf){
                if(currentTab > pages.Count) currentTab = 0;
                pages[currentTab].page.transform.SetAsLastSibling();
                pages[currentTab].page.transform.GetChild(0).GetComponent<Image>().color = new Color(240 / 255f, 240 / 255f, 1);
            } else {
                Destroy(itemDescription);
            }
            inventoryOpen.Invoke();
        }

        public static bool IsMenuActive(){
            return menu.activeSelf;
        }

        public static PageUI CreatePage(string name){
            if(pages.Count >= 10) return null;
            PageUI page = new PageUI(name);
            pages.Add(page);
            return page;
        }

        public static bool AddComponentDisplay(Inventory inventory){
            PageUI page = CreatePage(inventory.GetName());
            if(page == null) return false;
            InventoryUI inventoryUI = new InventoryUI();
            inventoryUI.CreateUI(inventory, page);
            return true;
        }

        public static void CreateHUD(Player player){
            // Create inventory page
            PageUI page = CreatePage("Inventory");
            PlayerInventoryUI playerInventory = new PlayerInventoryUI();
            playerInventory.CreateUI(player, page);

            // WorkBenchUI workBench = new WorkBenchUI();
            // workBench.CreateUI(new Inventory(8, 2, "input"), new Inventory(1, 1, "output"), CreatePage("Workbench"));

            // Create hotbar
            hotbar = new HotbarUI();
            hotbar.CreateUI(player);
        }

        public static void OnTabClick(GameObject click){
            click.transform.SetAsLastSibling();
            int index = 0;
            for(int i = 0; i < pages.Count; ++i) { 
                if(pages[i].page == click) index = i; 
            }
            pages[currentTab].page.transform.GetChild(0).GetComponent<Image>().color = new Color(190 / 255f, 190 / 255f, 215 / 255f);
            currentTab = index;
            click.transform.GetChild(0).GetComponent<Image>().color = new Color(240 / 255f, 240 / 255f, 1);
        }

        public static void ItemSlotClick(PointerEventData pointer, ItemSlot itemSlot, ItemStack newItem){
            if(newItem == null){
                if(heldItem != null){
                    CreateItemDescription(heldItem.GetComponent<HeldItem>().itemStack);
                    Destroy(heldItem);
                    heldItem = null;
                }
            } else {
                if(heldItem == null){
                    heldItem = Instantiate(slotPrefab);
                    Destroy(heldItem.GetComponent<Image>());
                    RectTransform heldTransform = heldItem.GetComponent<RectTransform>();
                    heldTransform.anchorMin = heldTransform.anchorMax = new Vector2(0, 1);
                    heldTransform.sizeDelta = new Vector2(Screen.height / 10f, Screen.height / 10f);
                    heldItem.transform.SetParent(menu.transform.parent);
                    heldItem.AddComponent<HeldItem>().SetItemStack(newItem);
                    Destroy(heldItem.GetComponent<ItemSlot>());
                } else {
                    heldItem.GetComponent<HeldItem>().SetItemStack(newItem);
                }
                Destroy(itemDescription);
            }
        }

        public static void CreateItemDescription(ItemStack item){
            if(item == null) return;
            itemDescription = Instantiate(descriptionPrefab);
            itemDescription.GetComponentInChildren<Text>().fontSize = Screen.height / 32;
            itemDescription.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height / 3.2f, 0);
            itemDescription.transform.SetParent(menu.transform.parent);
            string name = item.GetItem().itemData.Name;

            itemDescription.GetComponentInChildren<Text>().text = name;
        }

        public static void ItemSlotEnter(PointerEventData pointer, ItemSlot itemSlot){
            if(heldItem != null) return;
            CreateItemDescription(itemSlot.itemStack);
        }

        public static void ItemSlotExit(PointerEventData pointer, ItemSlot itemSlot){
            Destroy(itemDescription);
        }

        public static void AddInventoryOpenListener(UnityAction listener) {
            inventoryOpen.AddListener(listener);
        }
    }
}
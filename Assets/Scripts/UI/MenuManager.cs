using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Entities;
using UI.Elements;

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
        public static HeldItemUI itemHeld = null;
        public static DescriptorUI descriptor = null;
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
                for(int i = 1; i < pages.Count; ++i){
                    pages[i].DestroyUI();
                }
                if(pages.Count > 1) pages.RemoveRange(1, pages.Count - 1);
                currentTab = 0;
                if(descriptor != null) {
                    descriptor.DestroyUI();
                    descriptor = null;
                }
            }
            inventoryOpen.Invoke();
        }

        public static bool IsMenuActive(){
            return menu.activeSelf;
        }

        public static PageUI CreatePage(string name, System.Object obj){
            if(pages.Count >= 10) return null;
            foreach(PageUI p in pages) if(p.obj == obj) return null;
            PageUI page = new PageUI(name, obj);
            pages.Add(page);
            return page;
        }

        public static bool AddComponentDisplay(Inventory inventory){
            PageUI page = CreatePage(inventory.GetName(), inventory);
            if(page == null) return false;
            InventoryUI inventoryUI = new InventoryUI();
            inventoryUI.CreateUI(inventory, page);
            return true;
        }

        public static void CreateHUD(Player player){
            // Create inventory page
            PageUI page = CreatePage("Inventory", player);
            PlayerInventoryUI playerInventory = new PlayerInventoryUI();
            playerInventory.CreateUI(player, page);

            // WorkBenchUI workBench = new WorkBenchUI();
            // workBench.CreateUI(new Inventory(8, 2, "input"), new Inventory(1, 1, "output"), CreatePage("Workbench"));

            // Create hotbar
            hotbar = new HotbarUI();
            hotbar.CreateUI(player);
        }

        public static void OnTabClick(GameObject click){
            int index = 0;
            for(int i = 0; i < pages.Count; ++i) { 
                if(pages[i].page == click) index = i; 
            }
            SetTab(index);
        }

        public static void SetTab(int index){
            pages[index].page.transform.SetAsLastSibling();
            pages[currentTab].page.transform.GetChild(0).GetComponent<Image>().color = new Color(190 / 255f, 190 / 255f, 215 / 255f);
            currentTab = index;
            pages[index].page.transform.GetChild(0).GetComponent<Image>().color = new Color(240 / 255f, 240 / 255f, 1);
        }

        public static void SetHeldItem(ItemStack newItem){
            if(MenuManager.itemHeld == null){
                if(newItem != null){
                    if(MenuManager.descriptor != null) MenuManager.descriptor.DestroyUI();
                    MenuManager.itemHeld = new HeldItemUI(newItem); 
                }
            } else {
                if(newItem == null){
                    if(MenuManager.descriptor != null) {
                        MenuManager.descriptor.CreateUI();
                    }
                    MenuManager.itemHeld.Destroy();
                    MenuManager.itemHeld = null;
                } else {
                    MenuManager.itemHeld.Destroy();
                    MenuManager.itemHeld = new HeldItemUI(newItem);
                }
            }
        }

        public static void SetDescriptor(DescriptorUI descriptor){
            if(MenuManager.descriptor != null) MenuManager.descriptor.DestroyUI();
            MenuManager.descriptor = descriptor;
            if(itemHeld == null && descriptor != null) descriptor.CreateUI();
        }

        public static void AddInventoryOpenListener(UnityAction listener) {
            inventoryOpen.AddListener(listener);
        }
    }
}
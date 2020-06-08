using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MenuManager : MonoBehaviour
{   
    public GameObject serializedMenu;
    public GameObject serializedPagePrefab;
    public GameObject serializedSlotPrefab;
    public GameObject serializedDescriptionPrefab;
    
    public static GameObject menu;
    public static GameObject pagePrefab;
    public static GameObject slotPrefab;
    public static GameObject descriptionPrefab;

    static List<GameObject> pages = new List<GameObject>();

    private static MenuManager menuManager;
    private static GameObject emptyPrefab;

    private static int currentTab = 0;
    private static GameObject heldItem = null;
    private static GameObject itemDescription = null;
    private static UnityEvent inventoryOpen;
    private static HotbarUI hotbar;

    void Start() {
        MenuManager.menu = serializedMenu;
        MenuManager.pagePrefab = serializedPagePrefab;
        MenuManager.slotPrefab = serializedSlotPrefab;
        MenuManager.descriptionPrefab = serializedDescriptionPrefab;
        menuManager = this;
        emptyPrefab = new GameObject();
        RectTransform emptyRect = emptyPrefab.AddComponent<RectTransform>();
        emptyRect.anchorMin = Vector2.zero;
        emptyRect.anchorMax = Vector2.one;
        inventoryOpen = new UnityEvent();
    }

    public static void ToggleMenu(){
        menu.SetActive(!menu.activeSelf);
        if(menu.activeSelf){
            if(currentTab > pages.Count) currentTab = 0;
            pages[currentTab].transform.SetAsLastSibling();
            pages[currentTab].transform.GetChild(0).GetComponent<Image>().color = new Color(240 / 255f, 240 / 255f, 1);
        }
        inventoryOpen.Invoke();
    }

    public static bool IsMenuActive(){
        return menu.activeSelf;
    }

    public static bool AddComponentDisplay(Inventory inventory){
        if(pages.Count >= 10) return false;
        GameObject page = Instantiate(pagePrefab);
        page.transform.SetParent(menu.transform);

        GameObject tab = page.transform.GetChild(0).gameObject;
        RectTransform tabTransform = tab.GetComponent<RectTransform>();
        tabTransform.anchorMin = new Vector2(tabTransform.anchorMin.x, 0.9f - (pages.Count / 10f));
        tabTransform.anchorMax = new Vector2(tabTransform.anchorMax.x, 1f - (pages.Count / 10f));

        pages.Add(page);

        Button button = tab.GetComponent<Button>();
        button.GetComponentInChildren<Text>().text = inventory.GetName();
        button.onClick.AddListener(delegate { OnTabClick(page); });

        RectTransform pageTransform = page.GetComponent<RectTransform>();
        pageTransform.offsetMin = pageTransform.offsetMax = Vector3.zero;

        GameObject panel = page.transform.GetChild(1).gameObject;
        GameObject scroll = Instantiate(emptyPrefab);
        scroll.AddComponent<ScrollRect>();
        scroll.transform.SetParent(panel.transform);
        GameObject content = RectTransform.Instantiate(emptyPrefab);
        RectTransform contentTransform = content.GetComponent<RectTransform>();
        content.transform.SetParent(scroll.transform);
        scroll.GetComponent<ScrollRect>().content = content.GetComponent<RectTransform>();
        scroll.GetComponent<ScrollRect>().scrollSensitivity = 50;
        RectTransform scrollTransform = scroll.GetComponent<RectTransform>();
        scrollTransform.offsetMin = scrollTransform.offsetMax = Vector3.zero;

        content.AddComponent<InventoryUI>().CreateUI(inventory, slotPrefab);

        return true;
    }

    public static HotbarUI CreateHotbar(){
        GameObject content = RectTransform.Instantiate(emptyPrefab);
        content.transform.SetParent(menu.transform.parent);
        hotbar = content.AddComponent<HotbarUI>();
        Inventory inv = new Inventory(9, 1, "Hotbar");
        hotbar.CreateUI(inv, slotPrefab);
        return hotbar;
    }

    public static void OnTabClick(GameObject click){
        click.transform.SetAsLastSibling();
        int index = 0;
        for(int i = 0; i < pages.Count; ++i) { 
            if(pages[i] == click) index = i; 
        }
        pages[currentTab].transform.GetChild(0).GetComponent<Image>().color = new Color(190 / 255f, 190 / 255f, 215 / 255f);
        currentTab = index;
        click.transform.GetChild(0).GetComponent<Image>().color = new Color(240 / 255f, 240 / 255f, 1);
    }

    public static void ItemSlotClick(PointerEventData pointer, GameObject itemSlot){
        ItemStack newItem = itemSlot.transform.parent.GetComponent<ItemSlotHolderUI>().ClickSlot(heldItem, itemSlot);

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
                heldTransform.sizeDelta = new Vector2(100, 100);
                heldItem.transform.SetParent(menu.transform.parent);
                heldItem.AddComponent<HeldItem>().SetItemStack(newItem);
                Destroy(heldItem.GetComponent<ItemSlotUI>());
            } else {
                heldItem.GetComponent<HeldItem>().SetItemStack(newItem);
            }
            Destroy(itemDescription);
        }
    }

    public static void CreateItemDescription(ItemStack item){
        if(item == null) return;
        itemDescription = Instantiate(descriptionPrefab);
        itemDescription.transform.SetParent(menu.transform.parent);
        string name = item.GetItem().itemData.Name;

        itemDescription.GetComponentInChildren<Text>().text = name;
    }

    public static void ItemSlotEnter(PointerEventData pointer, GameObject itemSlot){
        Debug.Log("Enter");
        if(heldItem != null) return;
        CreateItemDescription(itemSlot.GetComponent<ItemSlotUI>().itemStack);
    }

    public static void ItemSlotExit(PointerEventData pointer, GameObject itemSlot){
        Debug.Log("Exit");
        Destroy(itemDescription);
    }

    public static void AddInventoryOpenListener(UnityAction listener) {
        inventoryOpen.AddListener(listener);
    }
}
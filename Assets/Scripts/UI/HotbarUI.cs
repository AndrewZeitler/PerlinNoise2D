using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Entities;
using UI.Elements;

namespace UI {
    public class HotbarUI {
        GameObject hotbar;
        Inventory inventory;
        int selectedSlot;
        public void CreateUI(Player player){
            this.inventory = player.inventory;
            selectedSlot = player.selectedSlot;

            GameObject slotPrefab = Resources.Load("Prefabs/ItemSlot") as GameObject;

            float aspectRatio = Screen.width / (float)Screen.height;

            hotbar = new GameObject();
            RectTransform uiTransform = hotbar.AddComponent<RectTransform>();
            uiTransform.anchorMin = new Vector2(1f - HotbarSlotUI.SLOT_SIZE.x, 0.05f);
            uiTransform.anchorMax = new Vector2(1f, 0.95f);
            hotbar.transform.SetParent(MenuManager.menu.transform.parent);

            for(int i = 0; i < Player.HOTBAR_SIZE; ++i){
                HotbarSlotUI itemSlot = new HotbarSlotUI(new Vector2(0, 1f - (1f / Player.HOTBAR_SIZE) * (i + 1)), i, player);
                itemSlot.gameObject.transform.SetParent(uiTransform);
                itemSlot.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                RectTransform slotTransform = itemSlot.gameObject.GetComponent<RectTransform>();
                slotTransform.offsetMin = slotTransform.offsetMax = Vector3.zero;
                slotTransform.anchorMax = new Vector2(1, 1f - (1f / Player.HOTBAR_SIZE) * i);
            }

            uiTransform.offsetMin = uiTransform.offsetMax = Vector2.zero;
            InventoryChanged();
            inventory.AddListener(InventoryChanged);
            player.AddSlotListener(OnSlotChange);
            MenuManager.AddInventoryOpenListener(InventoryOpened);
        }

        public void InventoryChanged(){
            for(int i = 0; i < hotbar.transform.childCount; ++i){
                ItemStack item = inventory.GetItemStackAt(i);
                Image image = hotbar.transform.GetChild(i).GetChild(0).GetComponent<Image>();
                Text text = hotbar.transform.GetChild(i).GetChild(1).GetComponent<Text>();
                hotbar.transform.GetChild(i).GetComponent<ItemSlot>().itemStack = item;
                hotbar.transform.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0.7f);
                if(item != null){
                    image.sprite = item.GetItem().itemData.ItemSprite;
                    image.color = new Color(1, 1, 1, 0.7f);
                    text.color = new Color(1, 1, 1, 0.7f);
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
                if(i == selectedSlot){
                    hotbar.transform.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    if(item != null){
                        image.color = new Color(1, 1, 1, 1);
                        text.color = new Color(1, 1, 1, 1);
                    }
                }
            }
        }

        public void InventoryOpened(){
            RectTransform uiTransform = hotbar.GetComponent<RectTransform>();
            if(MenuManager.IsMenuActive()){
                uiTransform.anchorMin = new Vector2(0.7f - (uiTransform.anchorMax.x - uiTransform.anchorMin.x), uiTransform.anchorMin.y);
                uiTransform.anchorMax = new Vector2(0.7f, uiTransform.anchorMax.y);
            } else {
                uiTransform.anchorMin = new Vector2(1f - (uiTransform.anchorMax.x - uiTransform.anchorMin.x), uiTransform.anchorMin.y);
                uiTransform.anchorMax = new Vector2(1f, uiTransform.anchorMax.y);
            }
        }

        public void OnSlotChange(int slot){
            if(MenuManager.IsMenuActive()) return;
            selectedSlot = slot;
            InventoryChanged();
        }
    }
}

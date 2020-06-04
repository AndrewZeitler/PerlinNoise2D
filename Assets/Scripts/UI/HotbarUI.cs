using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarUI : ItemSlotHolderUI
{
    private int selectedSlot = 0;

    public override void CreateUI(Inventory inventory, GameObject slotPrefab){
        this.inventory = inventory;

        float aspectRatio = Screen.width / (float)Screen.height;

        RectTransform uiTransform = GetComponent<RectTransform>();
        uiTransform.anchorMin = new Vector2(1f - 0.10f / aspectRatio, 0.05f);
        uiTransform.anchorMax = new Vector2(1f, 0.95f);

        for(int i = 0; i < inventory.GetInventorySize(); ++i){
            GameObject slot = Instantiate(slotPrefab);
            slot.transform.SetParent(transform);
            slot.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            slot.AddComponent<ItemSlotUI>().index = i;
            RectTransform slotTransform = slot.GetComponent<RectTransform>();
            slotTransform.offsetMin = slotTransform.offsetMax = Vector3.zero;
            slotTransform.anchorMin = new Vector2(0, 1f - (1f / inventory.GetInventorySize()) * (i + 1));
            slotTransform.anchorMax = new Vector2(1, 1f - (1f / inventory.GetInventorySize()) * i);
        }

        uiTransform.offsetMin = uiTransform.offsetMax = Vector2.zero;
        InventoryChanged();
        inventory.AddListener(InventoryChanged);
        MenuManager.AddInventoryOpenListener(InventoryOpened);
    }

    public override void InventoryChanged(){
        for(int i = 0; i < transform.childCount; ++i){
            ItemStack item = inventory.GetItemStackAt(i);
            Image image = transform.GetChild(i).GetChild(0).GetComponent<Image>();
            Text text = transform.GetChild(i).GetChild(1).GetComponent<Text>();
            transform.GetChild(i).GetComponent<ItemSlotUI>().itemStack = item;
            transform.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0.7f);
            if(item != null){
                image.sprite = SpriteManager.GetSprite(item.GetItem());
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
                transform.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                if(item != null){
                    image.color = new Color(1, 1, 1, 1);
                    text.color = new Color(1, 1, 1, 1);
                }
            }
        }
    }

    public override ItemStack ClickSlot(GameObject heldItem, GameObject clickedSlot){
        ItemStack item = null;
        if(heldItem != null){
            item = heldItem.GetComponent<HeldItem>().itemStack;
        }
        int slotIndex = clickedSlot.GetComponent<ItemSlotUI>().index;
        if(inventory.GetItemStackAt(slotIndex) == null){
            inventory.AddItemAt(slotIndex, item);
            return null;
        } else {
            ItemStack newHeldItem = inventory.RemoveItemAt(slotIndex);
            inventory.AddItemAt(slotIndex, item);
            return newHeldItem;
        }
    }

    public void InventoryOpened(){
        RectTransform uiTransform = GetComponent<RectTransform>();
        if(MenuManager.IsMenuActive()){
            uiTransform.anchorMin = new Vector2(0.7f - (uiTransform.anchorMax.x - uiTransform.anchorMin.x), uiTransform.anchorMin.y);
            uiTransform.anchorMax = new Vector2(0.7f, uiTransform.anchorMax.y);
        } else {
            uiTransform.anchorMin = new Vector2(1f - (uiTransform.anchorMax.x - uiTransform.anchorMin.x), uiTransform.anchorMin.y);
            uiTransform.anchorMax = new Vector2(1f, uiTransform.anchorMax.y);
        }
    }

    public ItemStack GetHeldItem(){
        return inventory.GetItemStackAt(selectedSlot);
    }

    // Update is called once per frame
    void Update()
    {
        float scroll = Input.mouseScrollDelta.y;
        if(scroll != 0f){
            if(scroll > 0f){
                selectedSlot += 1;
            } else {
                selectedSlot -= 1;
            }
            if(selectedSlot < 0) selectedSlot = inventory.GetInventorySize() - 1;
            if(selectedSlot >= inventory.GetInventorySize()) selectedSlot = 0;
            InventoryChanged();
        }
    }
}

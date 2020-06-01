using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;

    public void CreateUI(Inventory inventory, GameObject slotPrefab){
        this.inventory = inventory;

        float aspectRatio = Screen.width / (float)Screen.height;
        float slotHeight = (1f / inventory.GetColumns() * 0.3f * aspectRatio);
        RectTransform uiTransform = GetComponent<RectTransform>();
        uiTransform.anchorMax = new Vector2(1, slotHeight * (inventory.GetInventorySize() / inventory.GetColumns()));

        for(int i = 0; i < inventory.GetInventorySize(); ++i){
            GameObject slot = Instantiate(slotPrefab);
            slot.transform.SetParent(transform);
            slot.GetComponent<ItemSlotUI>().index = i;
            RectTransform slotTransform = slot.GetComponent<RectTransform>();
            slotTransform.offsetMin = slotTransform.offsetMax = Vector3.zero;
            slotTransform.anchorMin = new Vector2(1f / inventory.GetColumns() * (i % inventory.GetColumns()), 1f - slotHeight * (i / inventory.GetColumns() + 1) / uiTransform.anchorMax.y);
            slotTransform.anchorMax = new Vector2(1f / inventory.GetColumns() * (i % inventory.GetColumns() + 1), 1f - slotHeight * (i / inventory.GetColumns()) / uiTransform.anchorMax.y);
        }
        InventoryChanged();
        inventory.AddListener(InventoryChanged);
    }

    public void InventoryChanged(){
        for(int i = 0; i < transform.childCount; ++i){
            ItemStack item = inventory.GetItemStackAt(i);
            Image image = transform.GetChild(i).GetChild(0).GetComponent<Image>();
            Text text = transform.GetChild(i).GetChild(1).GetComponent<Text>();
            transform.GetChild(i).GetComponent<ItemSlotUI>().itemStack = item;
            if(item != null){
                image.sprite = SpriteManager.GetSprite(item.GetItem());
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

    public ItemStack ClickSlot(GameObject heldItem, GameObject clickedSlot){
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
}

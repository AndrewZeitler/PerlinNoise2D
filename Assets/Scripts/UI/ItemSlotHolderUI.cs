using UnityEngine;
using UnityEngine.UI;

public abstract class ItemSlotHolderUI : MonoBehaviour {
    public Inventory inventory;

    public abstract void CreateUI(Inventory inventory, GameObject slotPrefab);

    public abstract void InventoryChanged();

    public abstract ItemStack ClickSlot(GameObject heldItem, GameObject clickedSlot);
}
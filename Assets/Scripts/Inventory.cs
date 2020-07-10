using UnityEngine.Events;
using Items;

public class Inventory {

    string name;
    ItemSlot[] itemSlots;
    int cols;

    UnityEvent inventoryChanged;

    public Inventory(int slots, int cols, string name){
        if(slots % cols != 0) throw new System.Exception();
        itemSlots = new ItemSlot[slots];
        for(int i = 0; i < slots; ++i) {
            itemSlots[i] = new ItemSlot();
        }
        this.name = name;
        this.cols = cols;
        inventoryChanged = new UnityEvent();
    }

    public int GetInventorySize(){
        return itemSlots.Length;
    }

    public int GetColumns(){
        return cols;
    }

    public ItemSlot GetItemSlot(int index){
        return itemSlots[index];
    }

    public ItemStack GetItemStackAt(int index){
        return itemSlots[index].GetItemStack();
    }

    public bool AddItem(ItemStack item){
        foreach(ItemSlot slot in itemSlots){
            if(!slot.IsEmpty() && slot.GetItemStack().GetItem().itemData == item.GetItem().itemData) {
                if(slot.GetItemStack().Add(item)){
                    inventoryChanged.Invoke();
                    return true;
                }
            }
        }
        foreach(ItemSlot slot in itemSlots){
            if(slot.IsEmpty()) {
                slot.SetItemStack(item);
                inventoryChanged.Invoke();
                return true;
            }
        }
        return false;
    }

    public ItemStack AddItemAt(int index, ItemStack item){
        if(index < 0 || index > itemSlots.Length) throw new System.IndexOutOfRangeException();
        ItemStack prevItemStack = itemSlots[index].GetItemStack();
        if(prevItemStack != null && item != null && prevItemStack.GetItem().itemData == item.GetItem().itemData){
            if(prevItemStack.Add(item)){
                inventoryChanged.Invoke();
                return null;
            }
        }
        itemSlots[index].SetItemStack(item);
        inventoryChanged.Invoke();
        return prevItemStack;
    }

    public ItemStack RemoveItemAt(int index){
        ItemStack prevItemStack = itemSlots[index].GetItemStack();
        itemSlots[index].SetItemStack(null);
        inventoryChanged.Invoke();
        return prevItemStack;
    }

    public bool RemoveItemAt(int index, int amount){
        ItemStack itemStack = itemSlots[index].GetItemStack();
        if(itemStack.GetAmount() - amount <= 0) return false;
        itemSlots[index].SetItemStack(new ItemStack(itemStack.GetItem(), itemStack.GetAmount() - amount));
        inventoryChanged.Invoke();
        return true;
    }

    public int GetAmountOfType(Item item){
        int count = 0;
        foreach(ItemSlot slot in itemSlots){
            if(slot.GetItemStack().GetItem().itemData.Id == item.itemData.Id) count += slot.GetItemStack().GetAmount();
        }
        return count;
    }

    public string GetName(){
        return name;
    }

    public void AddListener(UnityAction call){
        inventoryChanged.AddListener(call);
    }

    public void RemoveListener(UnityAction call){
        inventoryChanged.RemoveListener(call);
    }
}
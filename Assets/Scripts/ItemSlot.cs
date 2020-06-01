
public class ItemSlot {
    ItemStack itemStack;

    public ItemSlot(){
        itemStack = null;
    }

    public ItemSlot(ItemStack itemStack){
        this.itemStack = itemStack;
    }

    public ItemStack GetItemStack(){
        return itemStack;
    }

    public ItemStack SetItemStack(ItemStack itemStack){
        ItemStack oldItemStack = this.itemStack;
        this.itemStack = itemStack;
        return oldItemStack;
    }

    public bool IsEmpty() { return itemStack == null; }
}
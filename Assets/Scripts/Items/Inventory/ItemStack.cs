using Items;

public class ItemStack {
    public static int StackLimit = 100;
    Item item;
    int amount;

    public ItemStack(Item item, int amount){
        if(amount <= 0) throw new System.ArgumentException("ItemStack can't have 0 or negative amount");
        this.item = item;
        this.amount = amount;
    }

    public ItemStack(ItemData itemData, int amount){
        if(amount <= 0) throw new System.ArgumentException("ItemStack can't have 0 or negative amount");
        this.item = new Item(itemData);
        this.amount = amount;
    }

    public Item GetItem(){
        return item;
    }

    public int GetAmount(){
        return amount;
    }

    public bool Add(ItemStack stack){
        if(amount + stack.amount > StackLimit) {
            stack.amount -= (StackLimit - amount);
            amount = StackLimit;
            return false;
        }
        amount += stack.amount;
        stack.amount = 0;
        return true;
    }

    public static int Compare(ItemStack obj, ItemStack other){
        int datacmp = ItemEnum.Compare(obj.GetItem().itemData, other.GetItem().itemData);
        if(datacmp != 0) return datacmp;
        return obj.amount.CompareTo(other.amount);
    }
}

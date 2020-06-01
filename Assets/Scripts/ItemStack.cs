
public class ItemStack {
    public static int StackLimit = 100;
    Item item;
    int amount;

    public ItemStack(Item item, int amount){
        this.item = item;
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
}
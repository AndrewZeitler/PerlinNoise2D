using Entities;

public class Hotbar {
    public Inventory inventory { get; }
    public int selectedSlot { get; set; }

    public Hotbar(Player player){
        inventory = new Inventory(9, 1, "Hotbar");
        selectedSlot = 0;
    }
}
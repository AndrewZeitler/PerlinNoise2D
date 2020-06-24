namespace Items {
    
    public class LootTable {
        public ItemStack[] loot;

        public LootTable(ItemStack[] loot){
            this.loot = loot;
        }

        public ItemStack GetDrop(){
            int rand = UnityEngine.Random.Range(0, loot.Length);
            return loot[rand];
        }

    }

}
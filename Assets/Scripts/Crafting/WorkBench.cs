using Items;

namespace Crafting {

    public class WorkBench : CraftingStation {

        public WorkBench(){
            RegisterRecipe(new Recipe(new ItemStack[]{new ItemStack(ItemData.WOOD, 2)}, new ItemStack(ItemData.WOOD, 1)));
            RegisterRecipe(new Recipe(new ItemStack[]{new ItemStack(ItemData.WOOD, 1), new ItemStack(ItemData.WORKBENCH, 1)}, new ItemStack(ItemData.WOOD, 3)));
        }

    }

}
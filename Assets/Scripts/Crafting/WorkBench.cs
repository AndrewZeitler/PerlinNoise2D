using Items;

namespace Crafting {

    public class WorkBench : CraftingData {

        public WorkBench(string name) : base(name){
            RegisterRecipe(new Recipe(new ItemStack[]{new ItemStack(ItemData.WOOD, 2)}, new ItemStack(ItemData.WOOD, 1)));
            RegisterRecipe(new Recipe(new ItemStack[]{new ItemStack(ItemData.WOOD, 4)}, new ItemStack(ItemData.WOOD, 2)));
            RegisterRecipe(new Recipe(new ItemStack[]{new ItemStack(ItemData.WOOD, 8)}, new ItemStack(ItemData.WOOD, 4)));
            RegisterRecipe(new Recipe(new ItemStack[]{new ItemStack(ItemData.WOOD, 16)}, new ItemStack(ItemData.WOOD, 8)));
            RegisterRecipe(new Recipe(new ItemStack[]{new ItemStack(ItemData.WOOD, 32)}, new ItemStack(ItemData.WOOD, 16)));
            RegisterRecipe(new Recipe(new ItemStack[]{new ItemStack(ItemData.WOOD, 64)}, new ItemStack(ItemData.WOOD, 32)));
            RegisterRecipe(new Recipe(new ItemStack[]{new ItemStack(ItemData.WOOD, 6)}, new ItemStack(ItemData.WOOD, 3)));
            RegisterRecipe(new Recipe(new ItemStack[]{new ItemStack(ItemData.WOOD, 12)}, new ItemStack(ItemData.WOOD, 6)));
            RegisterRecipe(new Recipe(new ItemStack[]{new ItemStack(ItemData.WOOD, 18)}, new ItemStack(ItemData.WOOD, 9)));
            RegisterRecipe(new Recipe(new ItemStack[]{new ItemStack(ItemData.WOOD, 24)}, new ItemStack(ItemData.WOOD, 12)));
            RegisterRecipe(new Recipe(new ItemStack[]{new ItemStack(ItemData.WOOD, 30)}, new ItemStack(ItemData.WOOD, 15)));
            RegisterRecipe(new Recipe(new ItemStack[]{new ItemStack(ItemData.WOOD, 1), new ItemStack(ItemData.WORKBENCH, 1)}, new ItemStack(ItemData.WOOD, 3)));
        }

    }

}
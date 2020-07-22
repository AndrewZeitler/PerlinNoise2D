
namespace Crafting {

    public class Recipe {

        public ItemStack[] ingredients { get; private set; }
        public ItemStack result { get; private set; }

        public Recipe(ItemStack[] ingredients, ItemStack result) { this.ingredients = ingredients; this.result = result; }

    }

}
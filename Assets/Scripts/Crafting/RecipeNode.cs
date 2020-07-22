using System.Collections.Generic;
using Items;

namespace Crafting {

    public class RecipeNode {
        public List<Recipe> result;
        public Dictionary<ItemData, RecipeNode> recipeList;

        public RecipeNode() {
            result = null;
            recipeList = null;
        }

        public RecipeNode(List<Recipe> result) {
            this.result = result;
            recipeList = null;
        }

        public RecipeNode(Dictionary<ItemData, RecipeNode> recipeList){
            this.recipeList = recipeList;
            result = null;
        }
    }

}
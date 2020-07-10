using System.Collections.Generic;
using Items;

namespace Crafting {

    public class RecipeNode {
        public Recipe result;
        public Dictionary<ItemData, RecipeNode> recipeList;

        public RecipeNode() {
            result = null;
            recipeList = null;
        }

        public RecipeNode(Recipe result) {
            this.result = result;
            recipeList = null;
        }

        public RecipeNode(Dictionary<ItemData, RecipeNode> recipeList){
            this.recipeList = recipeList;
            result = null;
        }
    }

}
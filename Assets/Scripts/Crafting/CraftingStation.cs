using System.Collections.Generic;
using Items;

namespace Crafting {

    public abstract class CraftingStation {
        protected bool isOrdered = false;
        public RecipeNode recipes = new RecipeNode();

        public virtual void RegisterRecipe(Recipe recipe){
            List<ItemData> items = new List<ItemData>();
            for(int i = 0; i < recipe.ingredients.Length; ++i){
                items.Add(recipe.ingredients[i].GetItem().itemData);
            }
            if(!isOrdered){
                items.Sort(ItemEnum.Compare);
            }
            RecipeNode node = recipes;
            for(int i = 0; i < items.Count; ++i){
                if(node.recipeList == null) node.recipeList = new Dictionary<ItemData, RecipeNode>();
                if(!node.recipeList.ContainsKey(items[i])){
                    node.recipeList[items[i]] = new RecipeNode();
                }
                node = node.recipeList[items[i]];
            }
            if(node.result == null) node.result = new List<Recipe>();
            node.result.Add(recipe);
        }

        public virtual List<List<Recipe>> GetRecipes(ItemStack[] itemStacks){
            if(itemStacks == null || itemStacks.Length == 0) return null;
            List<ItemData> items = new List<ItemData>();
            for(int i = 0; i < itemStacks.Length; ++i){
                items.Add(itemStacks[i].GetItem().itemData);
            }
            if(!isOrdered){
                items.Sort(ItemEnum.Compare);
            }

            RecipeNode end = recipes;
            while(end != null && items.Count > 0){
                if(end.recipeList == null) return null;
                if(!end.recipeList.ContainsKey(items[0])) return null;
                end = end.recipeList[items[0]];
                items.RemoveAt(0);
            }
            return DeepCopy(end);
        }

        protected List<List<Recipe>> DeepCopy(RecipeNode node){
            List<List<Recipe>> result  = new List<List<Recipe>>();
            if(node.recipeList == null) {
                result.Add(node.result);
            } else {
                if(node.result != null) result.Add(node.result);
                foreach(var item in node.recipeList){
                    result.AddRange(DeepCopy(item.Value));
                }
            }
            return result;
        }

        public bool CanMakeRecipe(Recipe recipe, List<ItemStack> items){
            if(recipe.ingredients.Length != items.Count) return false;

            List<ItemStack> ingredients = new List<ItemStack>();
            for(int i = 0; i < recipe.ingredients.Length; ++i){
                ingredients.Add(recipe.ingredients[i]);
            }
            if(!isOrdered) {
                items.Sort(ItemStack.Compare);
                ingredients.Sort(ItemStack.Compare);
            }

            for(int i = 0; i < items.Count; ++i){
                if(items[i].GetItem().itemData != ingredients[i].GetItem().itemData) return false;
                if(items[i].GetAmount() < ingredients[i].GetAmount()) return false;
            }
            
            return true;
        }
    }

}
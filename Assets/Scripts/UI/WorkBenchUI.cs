using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Crafting;
using UI.Elements;
using UnityEngine.EventSystems;

namespace UI {

    public class WorkBenchUI {
        static GameObject craftingPrefab = null;
        public GameObject gameObject { get; private set; }
        CraftingData craftingData;
        GameObject input;
        GameObject output;
        GameObject recipeGrid;
        Inventory craftingInventory;
        Inventory resultInventory;

        public void CreateUI(CraftingData craftingData, Inventory craftingInventory, Inventory resultInventory, PageUI page) {
            this.craftingData = craftingData;
            this.craftingInventory = craftingInventory;
            this.resultInventory = resultInventory;

            if(craftingPrefab == null) craftingPrefab = Resources.Load("Prefabs/Crafting") as GameObject;
            SceneManager.sceneManager.StartCoroutine(CreateCrafting(page));
        }

        public IEnumerator CreateCrafting(PageUI page){
            yield return new WaitForEndOfFrame();
            gameObject = GameObject.Instantiate(craftingPrefab);
            float width = page.viewport.GetComponent<RectTransform>().rect.width;

            input = gameObject.transform.GetChild(1).gameObject;
            output = gameObject.transform.GetChild(5).gameObject;
            gameObject.transform.GetChild(7).GetComponent<RectTransform>().sizeDelta = new Vector2(width, width);
            recipeGrid = gameObject.transform.GetChild(7).GetChild(0).GetChild(0).GetChild(0).gameObject;

            input.GetComponent<GridLayoutGroup>().cellSize = new Vector2(0.25f * width, 0.25f * width);
            output.GetComponent<GridLayoutGroup>().cellSize = new Vector2(0.25f * width, 0.25f * width);
            recipeGrid.GetComponent<GridLayoutGroup>().cellSize = new Vector2(0.5f * width, 0.25f * width);

            for(int i = 0; i < craftingInventory.GetInventorySize(); ++i){
                OpenSlotUI slot = new OpenSlotUI(i, craftingInventory);
                slot.SetParent(input.transform);
            }

            for(int i = 0; i < resultInventory.GetInventorySize(); ++i){
                TakeSlotUI slot = new TakeSlotUI(i, resultInventory);
                slot.SetParent(output.transform);
            }

            page.SetContent(gameObject);
            RectTransform transform = gameObject.GetComponent<RectTransform>();
            transform.offsetMax = transform.offsetMin = Vector2.zero;

            craftingInventory.AddListener(OnInputChange);
            resultInventory.AddListener(OnOutputChange);
        }

        public void OnInputChange(){
            for(int i = 0; i < recipeGrid.transform.childCount; ++i){
                recipeGrid.transform.GetChild(i).GetComponent<RecipeSlot>().recipeUI.Destroy();
            }

            List<ItemStack> items = new List<ItemStack>();
            for(int i = 0; i < craftingInventory.GetInventorySize(); ++i){
                if(craftingInventory.GetItemStackAt(i) != null){
                    items.Add(craftingInventory.GetItemStackAt(i));
                }
            }
            if(items.Count == 0) return;
            List<List<Recipe>> recipes = craftingData.GetRecipes(items.ToArray());
            //////////////////////////////////////////////////////////////////////////
            if(recipes == null || recipes.Count == 0) return;

            for(int i = 0; i < recipes[0].Count; ++i){
                bool canMakeRecipe = false;
                if(craftingData.CanMakeRecipe(recipes[0][i], items)) canMakeRecipe = true;
                RecipeUI recipeUI = new RecipeUI(recipes[0][i], canMakeRecipe);
                recipeUI.gameObject.transform.SetParent(recipeGrid.transform);
                recipeUI.AddClickListener(RecipeSlotClick);
            }
            for(int i = 1; i < recipes.Count; ++i){
                for(int j = 0; j < recipes[i].Count; ++j){
                    RecipeUI recipeUI = new RecipeUI(recipes[i][j], false);
                    recipeUI.gameObject.transform.SetParent(recipeGrid.transform);
                    recipeUI.AddClickListener(RecipeSlotClick);
                }
            }
        }

        public void OnOutputChange(){}

        public void RecipeSlotClick(PointerEventData pointer, RecipeSlot recipeSlot){
            if(recipeSlot.recipeUI.canMakeRecipe){
                if(resultInventory.AddItem(new ItemStack(recipeSlot.recipe.result.GetItem().itemData, recipeSlot.recipe.result.GetAmount()))){
                    for(int i = 0; i < recipeSlot.recipe.ingredients.Length; ++i){
                        craftingInventory.RemoveItem(recipeSlot.recipe.ingredients[i]);
                    }
                }
            }
        }

    }

}
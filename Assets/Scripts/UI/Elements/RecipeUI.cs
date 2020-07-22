using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Events;
using Crafting;
using UnityEngine.UI;

namespace UI.Elements {

    public class RecipeUI {
        public GameObject gameObject { get; private set; }
        static GameObject slotPrefab;
        RecipeSlotEvent OnSlotClick;
        public bool canMakeRecipe { get; private set; }
        ItemStack[] input;

        public RecipeUI(Recipe recipe, bool canMakeRecipe){
            this.canMakeRecipe = canMakeRecipe;
            if(slotPrefab == null) slotPrefab = Resources.Load("Prefabs/Recipe") as GameObject;
            gameObject = GameObject.Instantiate(slotPrefab, Vector3.zero, Quaternion.identity);
            if(!canMakeRecipe) {
                Color color = gameObject.GetComponent<Image>().color;
                gameObject.GetComponent<Image>().color = new Color(1, color.g - 0.1f, color.b - 0.1f, color.a);
            }
            gameObject.transform.GetChild(0).GetComponent<Image>().sprite = recipe.result.GetItem().itemData.ItemSprite;
            gameObject.transform.GetChild(1).GetComponent<Text>().text = "x " + recipe.result.GetAmount().ToString();
            RecipeSlot recipeSlot = gameObject.GetComponent<RecipeSlot>();
            recipeSlot.recipe = recipe;
            recipeSlot.recipeUI = this;
            recipeSlot.OnClick = ItemSlotClick;
            recipeSlot.OnEnter = ItemSlotEnter;
            recipeSlot.OnExit = ItemSlotExit;
            OnSlotClick = new RecipeSlotEvent();
        }

        public void AddClickListener(UnityAction<PointerEventData, RecipeSlot> listener) { 
            OnSlotClick.AddListener(listener);
        }

        public void ItemSlotClick(PointerEventData pointer, RecipeSlot recipeSlot){
            OnSlotClick.Invoke(pointer, recipeSlot);
        }

        public void ItemSlotEnter(PointerEventData pointer, RecipeSlot recipeSlot){
            if(canMakeRecipe){
                Color color = gameObject.GetComponent<Image>().color;
                gameObject.GetComponent<Image>().color = new Color(color.r + 0.075f, color.g + 0.075f, color.b + 0.075f, color.a);
            }
            MenuManager.SetDescriptor(new RecipeDescriptorUI(recipeSlot.recipe));
        }

        public void ItemSlotExit(PointerEventData pointer, RecipeSlot recipeSlot){
            if(canMakeRecipe){
                Color color = gameObject.GetComponent<Image>().color;
                gameObject.GetComponent<Image>().color = new Color(color.r - 0.075f, color.g - 0.075f, color.b - 0.075f, color.a);
            }
            if(MenuManager.descriptor != null) {
                MenuManager.descriptor.DestroyUI();
                MenuManager.descriptor = null;
            }
        }

        public void Destroy(){
            GameObject.Destroy(gameObject);
            if(MenuManager.descriptor as RecipeDescriptorUI != null) MenuManager.SetDescriptor(null);
        }

    }

}
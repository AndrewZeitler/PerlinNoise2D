using UnityEngine;
using UnityEngine.UI;
using Crafting;

namespace UI.Elements {

    public class RecipeDescriptorUI : DescriptorUI {
        static GameObject descriptionPrefab = null;
        static GameObject itemSlotPrefab = null;
        public GameObject gameObject { get; private set; }
        public Recipe recipe;

        public RecipeDescriptorUI(Recipe recipe){
            this.recipe = recipe;
        }

        public override void CreateUI(){
            if(descriptionPrefab == null) descriptionPrefab = Resources.Load("Prefabs/RecipeDescriptor") as GameObject;
            if(itemSlotPrefab == null) itemSlotPrefab = Resources.Load("Prefabs/ItemSlot") as GameObject;
            if(recipe == null) return;
            gameObject = GameObject.Instantiate(descriptionPrefab);
            gameObject.transform.GetChild(0).GetComponent<Text>().fontSize = Screen.height / 32;
            gameObject.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height / 3.2f, 0);
            gameObject.transform.SetParent(MenuManager.menu.transform.parent);

            string name = recipe.result.GetItem().itemData.Name;
            gameObject.GetComponentInChildren<Text>().text = name;
            GameObject ingredientList = gameObject.transform.GetChild(1).gameObject;
            ingredientList.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Screen.height / 3.2f * (31f / 36f) / 4, Screen.height / 3.2f * (31f / 36f) / 4);
            for(int i = 0; i < recipe.ingredients.Length; ++i){
                GameObject slot = GameObject.Instantiate(itemSlotPrefab);
                GameObject.Destroy(slot.GetComponent<ItemSlot>());
                Image image = slot.transform.GetChild(0).GetComponent<Image>();
                Text text = slot.transform.GetChild(1).GetComponent<Text>();
                slot.transform.SetParent(ingredientList.transform);
                ItemStack item = recipe.ingredients[i];
                if(item != null){
                    image.sprite = item.GetItem().itemData.ItemSprite;
                    image.color = new Color(1, 1, 1, 1);
                    if(item.GetAmount() > 1){
                        text.text = item.GetAmount().ToString();
                    } else {
                        text.text = "";
                    }
                } else {
                    image.sprite = null;
                    image.color = new Color(1, 1, 1, 0);
                    text.text = "";
                }
            }
        }

        public override void DestroyUI(){
            GameObject.Destroy(gameObject);
        }

    }

}
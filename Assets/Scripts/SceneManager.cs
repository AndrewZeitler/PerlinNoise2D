using UnityEngine;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour {
    public static SceneManager sceneManager { get; private set; } = null;
    public CameraController cam;
    public ProceduralGenerator generator;
    public Entities.Player player;

    public void Start(){
        sceneManager = this;
        generator.Initialize();
        player = new Entities.Player("Player", new Vector2(0, 0));
        UI.MenuManager.CreateHUD(player);
        cam.SetFollower(player.entityObject);
        generator.SetPlayer(cam.gameObject);

        List<List<Crafting.Recipe>> recipes = Crafting.CraftingData.WORKBENCH.GetRecipes(new ItemStack[]{new ItemStack(Items.ItemData.WOOD, 4)});
        foreach(var recipeList in recipes){
            foreach(var recipe in recipeList){
                Debug.Log("Result: " + recipe.result.GetItem().itemData.Name + "x" + recipe.result.GetAmount());
                foreach(var ingredient in recipe.ingredients){
                    Debug.Log("\t" + ingredient.GetItem().itemData.Name + "x" + ingredient.GetAmount());
                }
            }
        }
    }
}
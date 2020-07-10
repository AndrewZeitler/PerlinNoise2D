using UnityEngine;
using World;

public class SceneManager : MonoBehaviour {
    public CameraController cam;
    public ProceduralGenerator generator;
    public Entities.Player player;

    public void Start(){
        generator.Initialize();
        player = new Entities.Player("Player", new Vector2(0, 0));
        UI.MenuManager.CreateHUD(player);
        cam.SetFollower(player.entityObject);
        generator.SetPlayer(cam.gameObject);

        System.Collections.Generic.List<Crafting.Recipe> recipes = Crafting.CraftingData.WORKBENCH.GetRecipes(new ItemStack[]{new ItemStack(Items.ItemData.WOOD, 4)});
        foreach(var recipe in recipes){
            Debug.Log("Result: " + recipe.result.GetItem().itemData.Name + "x" + recipe.result.GetAmount());
            foreach(var ingredient in recipe.ingredients){
                Debug.Log("\t" + ingredient.GetItem().itemData.Name + "x" + ingredient.GetAmount());
            }
        }
    }

}
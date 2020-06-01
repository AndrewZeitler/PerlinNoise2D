using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public float fastSpeed;
    public int inventorySize;
    public int inventoryColumns;
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private Inventory inventory;
    private bool prevInteract = false;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        inventory = new Inventory(inventorySize, inventoryColumns, "Inventory");
        ItemManager itemManager = FindObjectOfType<ItemManager>();
        inventory.AddItem(new ItemStack(itemManager.GetItem("pickaxe"), 1));
        inventory.AddItem(new ItemStack(itemManager.GetItem("shovel"), 1));
        inventory.AddItem(new ItemStack(itemManager.GetItem("wood"), 10));
        MenuManager.AddComponentDisplay(inventory);
        MenuManager.AddComponentDisplay(new Inventory(inventorySize, inventoryColumns, "Test"));
    }

    void FixedUpdate()
    {
        if(prevInteract == false && Input.GetButton("Interact") == true){
            MenuManager.ToggleMenu();
            prevInteract = true;
        } else if(prevInteract == true && Input.GetButton("Interact") == false) prevInteract = false;
        if(!MenuManager.IsMenuActive()){
            float speed = movementSpeed;
            if(Input.GetAxisRaw("Fire3") > 0f) speed = fastSpeed;
            float xTranslation = Input.GetAxisRaw("Horizontal");
            float yTranslation = Input.GetAxisRaw("Vertical");
            Vector2 movement = new Vector2(xTranslation, yTranslation);
            body.velocity = movement.normalized * speed;
            short order = (short) Mathf.CeilToInt(-transform.position.y);
            spriteRenderer.sortingOrder = order;
        }
    }
}

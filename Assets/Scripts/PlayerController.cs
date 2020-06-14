using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public float fastSpeed;
    public float range;
    public int inventorySize;
    public int inventoryColumns;
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private Inventory inventory;
    private HotbarUI hotbar;
    private bool prevInteract = false;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        inventory = new Inventory(inventorySize, inventoryColumns, "Inventory");
        inventory.AddItem(new ItemStack(ItemData.PICKAXE, 10));
        inventory.AddItem(new ItemStack(ItemData.SHOVEL, 1));
        inventory.AddItem(new ItemStack(ItemData.WOOD, 10));
        hotbar = MenuManager.CreateHotbar();
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
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Floor(transform.position.y) + 0.5f);
        }
    }
}

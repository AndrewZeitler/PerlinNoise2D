using UnityEngine;
using World;
using Tiles;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public float fastSpeed;
    public float range;
    public int inventorySize;
    public int inventoryColumns;
    Rigidbody2D body;
    SpriteRenderer spriteRenderer;
    bool prevInteract = false;
    int prevButton = -1;
    Vector2Int prevMouse;
    

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update() {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)){
            int button = (Input.GetMouseButtonDown(0) ? 0 : 1);
            Debug.Log(button);
            Vector3 mouse = new Vector3((int)Input.mousePosition.x, (int)Input.mousePosition.y);
            mouse.z = Camera.main.transform.position.z;
            mouse = Camera.main.ScreenToWorldPoint(mouse);
            Vector2Int worldMouse = new Vector2Int(Mathf.FloorToInt(mouse.x + 0.5f), Mathf.FloorToInt(mouse.y + 0.5f));
            if(prevButton != -1 && (!worldMouse.Equals(prevMouse) || prevButton != button)){
                Tile prevTile = WorldManager.GetTile(prevMouse);
                if(prevButton == 0){
                    prevTile.ReleaseAttackEvent(WorldManager.player);
                } else {
                    prevTile.ReleaseInteractEvent(WorldManager.player);
                }
            }
            Tile tile = WorldManager.GetTile(worldMouse);
            Debug.Log(tile.tileData.Name);
            if(button == 0){
                tile.OnAttackEvent(WorldManager.player);
            } else {
                tile.OnInteractEvent(WorldManager.player);
            }
            prevMouse = worldMouse;
            prevButton = button;
        }

        if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)){
            Tile tile = WorldManager.GetTile(prevMouse);
            if(prevButton == 0){
                tile.ReleaseAttackEvent(WorldManager.player);
            } else {
                tile.ReleaseInteractEvent(WorldManager.player);
            }
            prevButton = -1;
        }
    }

    void FixedUpdate() {
        if(prevInteract == false && Input.GetButton("Interact") == true){
            MenuManager.ToggleMenu();
            prevInteract = true;
            body.velocity = Vector2.zero;
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

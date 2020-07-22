using UnityEngine;
using World;
using Tiles;
using Events;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public float fastSpeed;
    public float range;
    Rigidbody2D body;
    SpriteRenderer spriteRenderer;
    bool prevInteract = false;    
    SlotEvent onSlotChange = new SlotEvent();
    MouseEvent onAttack = new MouseEvent();
    MouseEvent onInteract = new MouseEvent();
    MouseEvent releaseAttack = new MouseEvent();
    MouseEvent releaseInteract = new MouseEvent();
    MouseEvent mouseMove = new MouseEvent();

    Vector3 prevMousePosition;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void AddSlotListener(UnityAction<int> listener){
        onSlotChange.AddListener(listener);
    }

    public void AddOnAttackListener(UnityAction<Vector3> listener){
        onAttack.AddListener(listener);
    }

    public void AddOnInteractListener(UnityAction<Vector3> listener){
        onInteract.AddListener(listener);
    }

    public void AddReleaseAttackListener(UnityAction<Vector3> listener){
        releaseAttack.AddListener(listener);
    }

    public void AddReleaseInteractListener(UnityAction<Vector3> listener){
        releaseInteract.AddListener(listener);
    }

    public void AddMouseMoveEvent(UnityAction<Vector3> listener){
        mouseMove.AddListener(listener);
    }

    void Update() {
        if(Input.GetMouseButtonDown(0)){
            onAttack.Invoke(Input.mousePosition);
        }
        if(Input.GetMouseButtonDown(1)){
            onInteract.Invoke(Input.mousePosition);
        }
        if(Input.GetMouseButtonUp(0)){
            releaseAttack.Invoke(Input.mousePosition);
        }
        if(Input.GetMouseButtonUp(1)){
            releaseInteract.Invoke(Input.mousePosition);
        }

        if(prevMousePosition != Input.mousePosition){
            mouseMove.Invoke(Input.mousePosition);
        }

        float scroll = Input.mouseScrollDelta.y;
        if(scroll != 0f){
            if(scroll > 0f){
                onSlotChange.Invoke(-1);
            } else {
                onSlotChange.Invoke(1);
            }
        }
        prevMousePosition = Input.mousePosition;
    }

    void FixedUpdate() {
        if(prevInteract == false && Input.GetButton("Interact") == true){
            UI.MenuManager.ToggleMenu();
            prevInteract = true;
            body.velocity = Vector2.zero;
        } else if(prevInteract == true && Input.GetButton("Interact") == false) prevInteract = false;
        
        float speed = movementSpeed;
        if(Input.GetAxisRaw("Fire3") > 0f) speed = fastSpeed;
        float xTranslation = Input.GetAxisRaw("Horizontal");
        float yTranslation = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(xTranslation, yTranslation);
        body.velocity = movement.normalized * speed;
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Floor(transform.position.y) + 0.5f);
    }
}

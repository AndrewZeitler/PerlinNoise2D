using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public float fastSpeed;
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        float speed = movementSpeed;
        if(Input.GetAxisRaw("Fire3") > 0f) speed = fastSpeed;
        float xTranslation = Input.GetAxisRaw("Horizontal");
        float yTranslation = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(xTranslation, yTranslation);
        // if(body.velocity.magnitude < movementSpeed){
        //     body.AddForce(movement.normalized * speed * Time.deltaTime);
        // }
        body.velocity = movement * speed;
        short order = (short) Mathf.CeilToInt(-transform.position.y);
        spriteRenderer.sortingOrder = order;
    }
}

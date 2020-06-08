using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 1.0f;
    public float fastSpeed = 1.0f;
    public float scrollSpeed = 1.0f;
    public GameObject player;
    private bool isFollowingPlayer;

    private Camera cam;
    
    void Start()
    {
        cam = GetComponent<Camera>();
        if(player != null) isFollowingPlayer = true;
    }

    void FixedUpdate()
    {
        if(!MenuManager.IsMenuActive()){
            float camSpeed = speed;
            if(Input.GetAxisRaw("Fire3") > 0f) camSpeed = fastSpeed;
            float xTranslation = Input.GetAxisRaw("Horizontal") * camSpeed;
            float yTranslation = Input.GetAxisRaw("Vertical") * camSpeed;
            xTranslation *= Time.deltaTime;
            yTranslation *= Time.deltaTime;
            float scroll = Input.mouseScrollDelta.y;
            if(Input.GetAxisRaw("Tab") > 0 && player != null) isFollowingPlayer = !isFollowingPlayer;
            if(scroll != 0f && !isFollowingPlayer){
                if(scroll > 0f){
                    cam.orthographicSize += scrollSpeed;
                } else {
                    cam.orthographicSize -= scrollSpeed;
                }
            }
            if(!isFollowingPlayer){
                transform.Translate(xTranslation, yTranslation, yTranslation);
            } else {
                xTranslation = player.transform.position.x - cam.transform.position.x;
                yTranslation = player.transform.position.y - cam.transform.position.y;
                transform.Translate(xTranslation, yTranslation, yTranslation);
            }
        }
    }
}

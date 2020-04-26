using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 1.0f;
    public float fastSpeed = 1.0f;
    public float scrollSpeed = 1.0f;
    // Start is called before the first frame update
    private Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float camSpeed = speed;
        if(Input.GetAxisRaw("Fire3") > 0f) camSpeed = fastSpeed;
        float xTranslation = Input.GetAxisRaw("Horizontal") * camSpeed;
        float yTranslation = Input.GetAxisRaw("Vertical") * camSpeed;
        xTranslation *= Time.deltaTime;
        yTranslation *= Time.deltaTime;
        float scroll = Input.mouseScrollDelta.y;
        if(scroll != 0f){
            if(scroll > 0f){
                cam.orthographicSize += scrollSpeed;
            } else {
                cam.orthographicSize -= scrollSpeed;
            }
        }

        transform.Translate(xTranslation, yTranslation, 0);
    }
}

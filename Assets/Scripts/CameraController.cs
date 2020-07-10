using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 1.0f;
    public float fastSpeed = 1.0f;
    public float scrollSpeed = 1.0f;
    public GameObject entity;
    private bool isFollowingEntity;

    private Camera cam;
    
    void Start()
    {
        cam = GetComponent<Camera>();
        if(entity != null) isFollowingEntity = true;
    }

    public void SetFollower(GameObject entity){
        this.entity = entity;
        isFollowingEntity = true;
        transform.position = new Vector3(entity.transform.position.x, entity.transform.position.y, transform.position.z);
    }

    void FixedUpdate()
    {
        float camSpeed = speed;
        if(Input.GetAxisRaw("Fire3") > 0f) camSpeed = fastSpeed;
        float xTranslation = Input.GetAxisRaw("Horizontal") * camSpeed;
        float yTranslation = Input.GetAxisRaw("Vertical") * camSpeed;
        xTranslation *= Time.deltaTime;
        yTranslation *= Time.deltaTime;
        float scroll = Input.mouseScrollDelta.y;
        if(Input.GetAxisRaw("Tab") > 0 && entity != null) isFollowingEntity = !isFollowingEntity;
        if(scroll != 0f && !isFollowingEntity){
            if(scroll > 0f){
                cam.orthographicSize += scrollSpeed;
            } else {
                cam.orthographicSize -= scrollSpeed;
            }
        }
        if(!isFollowingEntity){
            transform.Translate(xTranslation, yTranslation, yTranslation);
        } else {
            xTranslation = entity.transform.position.x - cam.transform.position.x;
            yTranslation = entity.transform.position.y - cam.transform.position.y;
            transform.Translate(xTranslation, yTranslation, yTranslation);
        }
    }
}

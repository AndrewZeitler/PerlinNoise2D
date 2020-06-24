using UnityEngine;
using World;

public class SceneManager : MonoBehaviour {
    public CameraController cam;
    public ProceduralGenerator generator;

    public void Start(){
        generator.Initialize();
        WorldManager.CreatePlayer();
        cam.SetFollower(WorldManager.player.entityObject);
        generator.SetPlayer(cam.gameObject);
    }

}
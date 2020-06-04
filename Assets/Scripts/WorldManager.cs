using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    #pragma warning disable 0649
    [SerializeField]
    ProceduralGenerator generator;
    [SerializeField]
    PlayerController player;
    #pragma warning restore 0649

    public Tile GetTileFromWorld(Vector3 coords){
        int xc = Mathf.FloorToInt(coords.x / Chunk.chunkSize) - generator.world[0,0].x;
        int yc = Mathf.FloorToInt(coords.y / Chunk.chunkSize) - generator.world[0,0].y;
        int x = Mathf.FloorToInt(coords.x % Chunk.chunkSize);
        int y = Mathf.FloorToInt(coords.y % Chunk.chunkSize);
        if(x < 0) x += Chunk.chunkSize;
        if(y < 0) y += Chunk.chunkSize;
        return generator.world[xc, yc].tiles[x, y];
    }

    void FixedUpdate() {
        if(Input.GetMouseButton(0)){
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0.5f, 0, 0);
            Debug.Log(mousePos);
            if(Vector2.Distance(player.transform.position, mousePos) < player.range){
                Debug.Log(GetTileFromWorld(mousePos).name);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
    public GameObject player;
    public WorldGenerator generator;
    Chunk[,] world;
    public int chunkSize = 16;
    public int chunkAmount = 5;

    Vector2 prevPlayerPos;

    void TranslateChunks(Vector2 dir){
        int x = (dir.x == -1 ? chunkAmount - 1 : 0);
        while(x >= 0 && x < chunkAmount){
            int y = (dir.y == -1 ? chunkAmount - 1 : 0);
            while(y >= 0 && y < chunkAmount){
                if(((x == 0 && dir.x == 1) || (x == chunkAmount - 1 && dir.x == -1)) || 
                   ((y == 0 && dir.y == 1) || (y == chunkAmount - 1 && dir.y == -1))) {
                       StartCoroutine(world[x, y].DestroyChunk());
                       world[x,y] = null;
                } else {
                    world[x - (int)dir.x, y - (int)dir.y] = world[x,y];
                    if(((x == 0 && dir.x == -1) || (x == chunkAmount - 1 && dir.x == 1)) || 
                       ((y == 0 && dir.y == -1) || (y == chunkAmount - 1 && dir.y == 1))){
                        world[x,y] = null;
                    }
                }
                y += (dir.y == -1 ? -1 : 1);
            }
            x += (dir.x == -1 ? -1 : 1);
        }
    }

    void GenerateNewTerrain(){
        for(int x = 0; x < chunkAmount; ++x){
            for(int y = 0; y < chunkAmount; ++y){
                if(world[x, y] != null) continue;
                world[x, y] = new Chunk(world[chunkAmount / 2, chunkAmount / 2].x + x - 2,
                                        world[chunkAmount / 2, chunkAmount / 2].y + y - 2);
                //StartCoroutine(generator.GenerateChunkConcurrently(world[x, y]));
                generator.GenerateChunk(world[x, y]);
            }
        }
    }

    void GenerateNewSprites(){
        for(int x = 1; x < chunkAmount - 1; ++x){
            for(int y = 1; y < chunkAmount - 1; ++y){
                if(world[x, y].grid[0,0].tile != null) continue;
                generator.MakeChunkSprites(world, x, y);
            }
        }
    }

    void Start()
    {
        world = new Chunk[chunkAmount, chunkAmount];
        for(int x = 0; x < chunkAmount; ++x){
            for(int y = 0; y < chunkAmount; ++y){
                world[x,y] = null;
            }
        }
        int xp = (int)player.transform.position.x / chunkSize;
        int yp = (int)player.transform.position.y / chunkSize;
        for(int x = 0; x < chunkAmount; ++x){
            for(int y = 0; y < chunkAmount; ++y){
                world[x, y] = new Chunk(xp + x - 2, yp + y - 2);
                generator.GenerateChunk(world[x, y]);
            }
        }
        GenerateNewSprites();
        prevPlayerPos = new Vector2(xp, yp);
    }

    void FixedUpdate()
    {
        int xp = (int)Mathf.Floor(player.transform.position.x / chunkSize);
        int yp = (int)Mathf.Floor(player.transform.position.y / chunkSize);
        if(xp != prevPlayerPos.x || yp != prevPlayerPos.y){
            Vector2 dir = new Vector2(xp - prevPlayerPos.x, yp - prevPlayerPos.y);
            TranslateChunks(dir);
            GenerateNewTerrain();
            GenerateNewSprites();
            prevPlayerPos.x = xp;
            prevPlayerPos.y = yp;
        }
    }
}

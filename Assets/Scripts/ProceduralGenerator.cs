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

    Dictionary<Vector2, Chunk> loadedChunks;

    Vector2 prevPlayerPos;

    void TranslateChunks(Vector2 dir){
        int x = (dir.x == -1 ? chunkAmount - 1 : 0);
        while(x >= 0 && x < chunkAmount){
            int y = (dir.y == -1 ? chunkAmount - 1 : 0);
            while(y >= 0 && y < chunkAmount){
                if(((x == 0 && dir.x == 1) || (x == chunkAmount - 1 && dir.x == -1)) || 
                   ((y == 0 && dir.y == 1) || (y == chunkAmount - 1 && dir.y == -1))) {
                       StartCoroutine(world[x, y].DestroyChunk());
                       if(world[x,y].chunkState == ChunkState.Rendered) world[x,y].chunkState = ChunkState.Smoothed;
                       if(!loadedChunks.ContainsKey(new Vector2(world[x,y].x, world[x,y].y))){
                           loadedChunks[new Vector2(world[x,y].x, world[x,y].y)] = world[x,y];
                       }
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

    // void GenerateNewTerrain(){
    //     for(int x = 0; x < chunkAmount; ++x){
    //         for(int y = 0; y < chunkAmount; ++y){
    //             if(world[x, y] != null) continue;
    //             world[x, y] = new Chunk(world[chunkAmount / 2, chunkAmount / 2].x + x - 2,
    //                                     world[chunkAmount / 2, chunkAmount / 2].y + y - 2);
    //             //StartCoroutine(generator.GenerateChunkConcurrently(world[x, y]));
    //             generator.GenerateChunk(world[x, y]);
    //         }
    //     }
    // }

    // void GenerateNewSprites(){
    //     for(int x = 1; x < chunkAmount - 1; ++x){
    //         for(int y = 1; y < chunkAmount - 1; ++y){
    //             if(world[x, y].grid[0,0].tile != null) continue;
    //             //generator.MakeChunkSprites(world, x, y);
    //         }
    //     }
    // }

    void Start()
    {
        loadedChunks = new Dictionary<Vector2, Chunk>();
        Chunk.chunkSize = chunkSize;
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
                world[x, y] = new Chunk(xp + x - chunkAmount / 2, yp + y - chunkAmount / 2);
                //generator.GenerateChunk(world[x, y]);
            }
        }
        //GenerateNewSprites();
        generator.LoadChunks(world);
        prevPlayerPos = new Vector2(xp, yp);
    }

    void GenerateNewTerrain(int xp, int yp){
        for(int x = 0; x < chunkAmount; ++x){
            for(int y = 0; y < chunkAmount; ++y){
                if(world[x,y] == null){
                    if(loadedChunks.ContainsKey(new Vector2(xp + x - chunkAmount / 2, yp + y - chunkAmount / 2))){
                        world[x,y] = loadedChunks[new Vector2(xp + x - chunkAmount / 2, yp + y - chunkAmount / 2)];
                    } else {
                        world[x,y] = new Chunk(xp + x - chunkAmount / 2, yp + y - chunkAmount / 2);
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        int xp = (int)Mathf.Floor(player.transform.position.x / Chunk.chunkSize);
        int yp = (int)Mathf.Floor(player.transform.position.y / Chunk.chunkSize);
        if(xp != prevPlayerPos.x || yp != prevPlayerPos.y){
            Vector2 dir = new Vector2(xp - prevPlayerPos.x, yp - prevPlayerPos.y);
            TranslateChunks(dir);
            GenerateNewTerrain(xp, yp);
            generator.LoadChunks(world);
            prevPlayerPos.x = xp;
            prevPlayerPos.y = yp;
        }
    }
}

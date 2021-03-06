﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

public class ProceduralGenerator : MonoBehaviour
{
    GameObject player;
    WorldGenerator generator;
    public Chunk[,] world;
    public int chunkSize;
    public int loadWidth;
    public int loadHeight;
    public int collisionRadius;

    Vector2 prevPlayerPos;

    bool worldIsGenerated = false;
    bool playerCreated = false;

    void TranslateChunks(Vector2 dir){
        int x = (dir.x == -1 ? loadWidth - 1 : 0);
        while(x >= 0 && x < loadWidth){
            int y = (dir.y == -1 ? loadHeight - 1 : 0);
            while(y >= 0 && y < loadHeight){
                if(((x == 0 && dir.x == 1) || (x == loadWidth - 1 && dir.x == -1)) || 
                   ((y == 0 && dir.y == 1) || (y == loadHeight - 1 && dir.y == -1))) {
                       StartCoroutine(world[x, y].DestroyChunk());
                       if(world[x,y].chunkState == ChunkState.Rendered || world[x,y].chunkState == ChunkState.Rendering) world[x,y].chunkState = ChunkState.Generated;
                       world[x,y] = null;
                } else {
                    world[x - (int)dir.x, y - (int)dir.y] = world[x,y];
                    if(((x == 0 && dir.x == -1) || (x == loadWidth - 1 && dir.x == 1)) || 
                       ((y == 0 && dir.y == -1) || (y == loadHeight - 1 && dir.y == 1))){
                            world[x,y] = null;
                    }
                }
                y += (dir.y == -1 ? -1 : 1);
            }
            x += (dir.x == -1 ? -1 : 1);
        }
    }

    public void Initialize() {
        // seed: 3736690
        generator = new WorldGenerator();
        Chunk.chunkSize = chunkSize;
        world = new Chunk[loadWidth, loadHeight];
        for(int x = 0; x < loadWidth; ++x){
            for(int y = 0; y < loadHeight; ++y){
                world[x,y] = null;
            }
        }
        for(int x = 0; x < loadWidth; ++x){
            for(int y = 0; y < loadHeight; ++y){
                world[x, y] = new Chunk(x - loadWidth / 2, y - loadHeight / 2);
                WorldManager.AddChunk(new Vector2(world[x, y].x, world[x, y].y), world[x, y]);
            }
        }
        generator.LoadChunks(world);
        worldIsGenerated = true;
    }

    public void SetPlayer(GameObject player){
        this.player = player;
        int xp = (int)player.transform.position.x / chunkSize;
        int yp = (int)player.transform.position.y / chunkSize;
        prevPlayerPos = new Vector2(xp, yp);
        // for(int x = 0; x < loadWidth; ++x){
        //     for(int y = 0; y < loadHeight; ++y){
        //         //StartCoroutine(world[x, y].DestroyChunk());
        //         world[x, y] = null;
        //     }
        // }
        GenerateNewTerrain(xp, yp);
        generator.LoadChunks(world);
        playerCreated = true;
    }

    void GenerateNewTerrain(int xp, int yp){
        for(int x = 0; x < loadWidth; ++x){
            for(int y = 0; y < loadHeight; ++y){
                if(world[x,y] == null){
                    Chunk chunk = WorldManager.GetChunk(new Vector2(xp + x - loadWidth / 2, yp + y - loadHeight / 2));
                    if(chunk != null){
                        world[x,y] = chunk;
                    } else {
                        world[x,y] = new Chunk(xp + x - loadWidth / 2, yp + y - loadHeight / 2);
                        WorldManager.AddChunk(new Vector2(world[x, y].x, world[x, y].y), world[x, y]);
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if(!worldIsGenerated || !playerCreated) return;
        //UpdateColliders(player.transform.position.x, player.transform.position.y);
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

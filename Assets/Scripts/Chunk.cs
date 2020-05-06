using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {
    public int x;
    public int y;
    public static int chunkSize = 16;
    public Tile[,] grid;
    public ChunkState chunkState;

    public Chunk(int x, int y){
        this.x = x;
        this.y = y;
        grid = new Tile[chunkSize, chunkSize];
        chunkState = ChunkState.NotGenerated;
    }

    public IEnumerator DestroyChunk(){
        for(int x = 0; x < chunkSize; ++x){
            for(int y = 0; y < chunkSize; ++y){
                grid[x,y].DestroyTile();
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
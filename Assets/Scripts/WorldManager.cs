using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tiles;
using Entities;

namespace World {
    public static class WorldManager {
        public static Player player { get; private set; }
        static Dictionary<Vector2, Chunk> loadedChunks = new Dictionary<Vector2, Chunk>();

        public static void CreatePlayer(){
            // Vector2 pos = new Vector2();
            // bool foundPos = false;
            // while(!foundPos){
            //     int rand = Random.Range(0, loadedChunks.Count);
            //     foreach(var chunk in loadedChunks){
            //         if(rand == 0){
            //             int x = Random.Range(0, Chunk.chunkSize);
            //             int y = Random.Range(0, Chunk.chunkSize);
            //             if(chunk.Value.terrain[x,y].tileData.IsWalkable && chunk.Value.tiles[x,y].tileData.IsWalkable){
            //                 Debug.Log(new Vector2(chunk.Value.x, chunk.Value.y));
            //                 pos = new Vector2(chunk.Value.x * Chunk.chunkSize + x, chunk.Value.y * Chunk.chunkSize + y);
            //                 foundPos = true;
            //             }
            //             break;
            //         } else {
            //             --rand;
            //         }
            //     }
            // }
            // Debug.Log(pos);
            player = new Player("Player", new Vector2(0, 0));
            MenuManager.AddComponentDisplay(player.inventory);
            MenuManager.CreateHotbar(player.hotbar);
        }

        public static void AddChunk(Vector2 pos, Chunk chunk){
            loadedChunks.Add(pos, chunk);
        }

        public static Chunk GetChunk(Vector2 pos){
            if(!loadedChunks.ContainsKey(pos)) return null;
            return loadedChunks[pos];
        }

        public static Tile GetTile(Vector2 pos){
            int xc = Mathf.FloorToInt(pos.x / Chunk.chunkSize);
            int yc = Mathf.FloorToInt(pos.y / Chunk.chunkSize);
            int x = Mathf.FloorToInt(pos.x % Chunk.chunkSize);
            int y = Mathf.FloorToInt(pos.y % Chunk.chunkSize);
            if(x < 0) x += Chunk.chunkSize;
            if(y < 0) y += Chunk.chunkSize;
            return loadedChunks[new Vector2(xc, yc)].tiles[x, y];
        }

        public static Tile[,] GetTerrainGrid(Vector2 pos, Vector2Int size){
            Tile[,] grid = new Tile[size.x, size.y];
            for(int x = 0; x < size.x; ++x){
                for(int y = 0; y < size.y; ++y){
                    Tile tile = GetTerrain(new Vector2(pos.x + x, pos.y + y));
                    if(tile == null) return null;
                    grid[x, y] = tile;
                }
            }
            return grid;
        }

        public static Tile GetTerrain(Vector2 pos){
            int xc = Mathf.FloorToInt(pos.x / Chunk.chunkSize);
            int yc = Mathf.FloorToInt(pos.y / Chunk.chunkSize);
            int x = Mathf.FloorToInt(pos.x % Chunk.chunkSize);
            int y = Mathf.FloorToInt(pos.y % Chunk.chunkSize);
            if(x < 0) x += Chunk.chunkSize;
            if(y < 0) y += Chunk.chunkSize;
            return loadedChunks[new Vector2(xc, yc)].terrain[x, y];
        }
    }
}

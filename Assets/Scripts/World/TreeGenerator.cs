using Tiles;
using System.Collections.Generic;
using UnityEngine;

namespace World {

    public class TreeGenerator : GeneratorModifier {

        TileData tileData;
        HashSet<int> spawnIds;
        Vector2 veinHeight;
        float singleChance;
        float veinChance;

        public TreeGenerator(int priority, TileData tileData, float singleChance, Vector2 veinHeight, float veinChance, TileData[] spawnTiles) : base(priority) {
            this.tileData = tileData;
            this.veinHeight = veinHeight;
            this.singleChance = singleChance;
            this.veinChance = veinChance;
            spawnIds = new HashSet<int>();
            foreach(TileData tile in spawnTiles) { spawnIds.Add(tile.Id); }
        }

        public override void AfterGenerate(Chunk chunk){
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    if(!spawnIds.Contains(chunk.terrain[x, y].tileData.Id)) continue;
                    //if(chunk.tiles[x,y].tileData.Id != 0) continue;
                    double rand = Random.value;
                    if(rand < singleChance){
                        chunk.tiles[x,y].SetTileData(tileData);
                        continue;
                    }
                    if(chunk.heightMap[x,y] > veinHeight.x && chunk.heightMap[x,y] < veinHeight.y){
                        rand = Random.value;
                        if(rand < veinChance){
                            chunk.tiles[x,y].SetTileData(tileData);
                        }
                    }
                }
            }
        }

    }

}
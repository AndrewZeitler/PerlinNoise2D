using Tiles;
using System.Collections.Generic;
using UnityEngine;

namespace World {

    public class ResourceGenerator : GeneratorModifier {

        TileData tileData;
        HashSet<int> spawnIds;
        float resourceChance;
        float veinChance;

        public ResourceGenerator(int priority, TileData tileData, float resourceChance, float veinChance, TileData[] spawnTiles) : base(priority) {
            this.tileData = tileData;
            this.resourceChance = resourceChance;
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
                    if(rand < resourceChance){
                        chunk.tiles[x,y].SetTileData(tileData);
                    } else {
                        rand = WorldGenerator.PerlinNoise(x + chunk.x * Chunk.chunkSize, y + chunk.y * Chunk.chunkSize, tileData.Id);
                        if(rand < veinChance){
                            chunk.tiles[x,y].SetTileData(tileData);
                        }
                    }
                }
            }
        }

    }

}
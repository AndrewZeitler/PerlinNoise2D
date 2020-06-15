using Tiles;
using System.Collections.Generic;
using UnityEngine;

namespace World {

    public class VariationGenerator : GeneratorModifier {

        TileData tileData;
        HashSet<int> spawnIds;
        float spawnChance;

        public VariationGenerator(int priority, TileData tileData, float spawnChance, TileData[] spawnTiles) : base(priority) {
            this.tileData = tileData;
            this.spawnChance = spawnChance;
            spawnIds = new HashSet<int>();
            foreach(TileData tile in spawnTiles) { spawnIds.Add(tile.Id); }
        }

        public override void AfterGenerate(Chunk chunk){
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    if(!spawnIds.Contains(chunk.terrain[x, y].tileData.Id)) continue;
                    double rand = Random.value;
                    if(rand < spawnChance){
                        chunk.tiles[x,y].SetTileData(tileData);
                    }
                }
            }
        }

    }

}
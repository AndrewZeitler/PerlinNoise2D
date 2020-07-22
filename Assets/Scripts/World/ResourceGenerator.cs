using Tiles;
using System.Collections.Generic;
using UnityEngine;

namespace World {

    public class ResourceGenerator : GeneratorModifier {

        TileData tileData;
        HashSet<int> spawnIds;
        Vector2 height;
        float spawnChance;
        float veinChance;

        public ResourceGenerator(int priority, TileData tileData, Vector2 height, float spawnChance, float veinChance, TileData[] spawnTiles) : base(priority) {
            this.tileData = tileData;
            this.height = height;
            this.spawnChance = spawnChance;
            this.veinChance = veinChance;
            spawnIds = new HashSet<int>();
            foreach(TileData tile in spawnTiles) { spawnIds.Add(tile.Id); }
        }

        public override void AfterGenerate(Chunk chunk, int x, int y){
            if(!spawnIds.Contains(chunk.terrain[x, y].tileData.Id)) return;
            //if(chunk.tiles[x,y].tileData.Id != 0) continue;
            double rand = chunk.heightMap[x, y];
            if(rand > height.x && rand < height.y){
                rand = Random.value;
                if(rand < veinChance){
                    chunk.tiles[x,y].SetTileData(tileData);
                }
                chunk.tiles[x,y].SetTileData(tileData);
            }
        }

    }

}
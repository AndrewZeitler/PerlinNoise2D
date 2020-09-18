using Tiles;
using Utils;
using UnityEngine;

namespace World.Biomes.Modifiers
{
    
    public class ResourceModifier : BiomeModifier {
        float surviveChance;
        TileData resourceTile;
        TileData spawnTile;

        public ResourceModifier(int priority, float surviveChance, TileData resourceTile, TileData spawnTile=null) : base(priority) { 
            this.surviveChance = surviveChance;
            this.spawnTile = spawnTile;
            this.resourceTile = resourceTile;
        }

        public override void OnGenerate(Chunk chunk, int x, int y) {
            if((chunk.terrain[x,y].tileData == spawnTile || spawnTile == null) && chunk.tiles[x, y].tileData == TileData.AIR) {
                if(Random.value < surviveChance) chunk.tiles[x, y].SetTileData(resourceTile);
            }
        }

    }

}
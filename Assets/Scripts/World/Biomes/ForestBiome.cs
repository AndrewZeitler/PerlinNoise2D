using World.Biomes.Modifiers;
using Tiles;

namespace World.Biomes
{
    public class ForestBiome : Biome {

        public ForestBiome(float perlinZoom, float spawnChance, int seed) : base("Forest", 5, perlinZoom, spawnChance, seed) {
            AddModifier(new SolidTerrainModifier(0, TileData.GRASS));
            AddModifier(new ResourceModifier(0, 0.4f, TileData.TREE, TileData.GRASS));
        }

        public override bool CanSpawn(Chunk chunk, int x, int y){
            bool result = base.CanSpawn(chunk, x, y);
            return (result && chunk.terrain[x, y].tileData.Equals(Tiles.TileData.AIR));
        }

    }
}
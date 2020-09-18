using World.Biomes.Modifiers;

namespace World.Biomes
{
    public class GrasslandBiome : Biome {

        public GrasslandBiome(float perlinZoom, float spawnChance, int seed) : base("Grasslands", 1, perlinZoom, spawnChance, seed) {
            AddModifier(new SolidTerrainModifier(0, Tiles.TileData.GRASS));
            AddModifier(new ResourceModifier(1, 0.015f, Tiles.TileData.TREE, Tiles.TileData.GRASS));
            AddModifier(new ResourceModifier(1, 0.005f, Tiles.TileData.DIRT_ROCK, Tiles.TileData.GRASS));
            AddModifier(new ResourceModifier(1, 0.005f, Tiles.TileData.STONE_ROCK, Tiles.TileData.GRASS));
        }

    }
}
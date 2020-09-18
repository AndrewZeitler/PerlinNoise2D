using World.Biomes.Modifiers;

namespace World.Biomes
{
    public class OceanBiome : Biome {

        public OceanBiome(float perlinZoom, float spawnChance, int seed) : base("Ocean", 0, perlinZoom, spawnChance, seed) {
            AddModifier(new SolidTerrainModifier(0, Tiles.TileData.WATER));
            AddModifier(new ResourceModifier(1, 0.005f, Tiles.TileData.LILYPAD, Tiles.TileData.GRASS));
        }

    }
}
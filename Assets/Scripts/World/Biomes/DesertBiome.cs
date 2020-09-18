using World.Biomes.Modifiers;

namespace World.Biomes
{
    public class DesertBiome : Biome {

        public DesertBiome(float perlinZoom, float spawnChance, int seed) : base("Desert", 4, perlinZoom, spawnChance, seed) {
            AddModifier(new SolidTerrainModifier(0, Tiles.TileData.SAND));
        }

        public override bool CanSpawn(Chunk chunk, int x, int y){
            bool result = base.CanSpawn(chunk, x, y);
            return (result && chunk.terrain[x, y].tileData.Equals(Tiles.TileData.AIR));
        }
    }
}
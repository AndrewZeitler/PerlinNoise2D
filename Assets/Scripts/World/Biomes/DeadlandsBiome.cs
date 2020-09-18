using World.Biomes.Modifiers;

namespace World.Biomes
{
    public class DeadlandsBiome : Biome {

        public DeadlandsBiome(float perlinZoom, float spawnChance, int seed) : base("Deadlands", 3, perlinZoom, spawnChance, seed) {
            AddModifier(new SolidTerrainModifier(0, Tiles.TileData.DEADGRASS));
        }

        public override bool CanSpawn(Chunk chunk, int x, int y){
            bool result = base.CanSpawn(chunk, x, y);
            return (result && chunk.terrain[x, y].tileData.Equals(Tiles.TileData.AIR));
        }

    }
}
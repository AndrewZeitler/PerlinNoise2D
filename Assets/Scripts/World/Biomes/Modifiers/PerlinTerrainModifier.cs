using Tiles;
using Utils;

namespace World.Biomes.Modifiers
{
    
    public class PerlinTerrainModifier : BiomeModifier {
        PerlinGenerator perlinGenerator;
        TileData solidTile;

        public PerlinTerrainModifier(int priority, TileData solidTile, float perlinZoom, float spawnChance, int seed) : base(priority) { 
            this.solidTile = solidTile; 
            perlinGenerator = new PerlinGenerator(perlinZoom, spawnChance, seed);    
        }

        public override void OnGenerate(Chunk chunk, int x, int y) {
            if(perlinGenerator.CanSpawn(x, y)) chunk.terrain[x, y].SetTileData(solidTile);
        }

    }

}
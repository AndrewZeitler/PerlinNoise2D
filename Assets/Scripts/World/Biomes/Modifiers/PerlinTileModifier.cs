using Tiles;
using Utils;

namespace World.Biomes.Modifiers
{
    
    public class PerlinTileModifier : BiomeModifier {
        PerlinGenerator perlinGenerator;
        TileData solidTile;
        TileData spawnTerrain;

        public PerlinTileModifier(int priority, TileData solidTile, float perlinZoom, float spawnChance, int seed, TileData spawnTerrain = null) : base(priority) { 
            this.solidTile = solidTile;
            this.spawnTerrain = spawnTerrain;
            perlinGenerator = new PerlinGenerator(perlinZoom, spawnChance, seed);    
        }

        public override void OnGenerate(Chunk chunk, int x, int y) {
            if(spawnTerrain != null && !chunk.terrain[x, y].tileData.Equals(spawnTerrain)) return;
            if(perlinGenerator.CanSpawn(x, y)) chunk.terrain[x, y].SetTileData(solidTile);
        }

    }

}
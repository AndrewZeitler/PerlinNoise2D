using Tiles;

namespace World.Biomes.Modifiers
{
    
    public class SolidTerrainModifier : BiomeModifier {
        TileData solidTile;
        TileData spawnTile;

        public SolidTerrainModifier(int priority, TileData solidTile, TileData spawnTile=null) : base(priority) { this.solidTile = solidTile; this.spawnTile = spawnTile; }

        public override void BeforeGenerate(Chunk chunk, int x, int y) {
            if(spawnTile != null && !chunk.terrain[x, y].tileData.Equals(spawnTile)) return;
            chunk.terrain[x, y].SetTileData(solidTile);
        }

    }

}
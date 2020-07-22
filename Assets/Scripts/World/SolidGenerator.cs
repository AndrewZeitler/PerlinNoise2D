using Tiles;

namespace World {

    public class SolidGenerator : GeneratorModifier {
        TileData tile;
        public SolidGenerator(int priority, TileData tile) : base(priority){this.tile = tile;}

        public override void OnGenerate(Chunk chunk, int x, int y){
            chunk.terrain[x,y].SetTileData(tile);
        }

    }

}
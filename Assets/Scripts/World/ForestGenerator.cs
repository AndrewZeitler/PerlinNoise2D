using Tiles;

namespace World {

    public class ForestGenerator : GeneratorModifier {

        public ForestGenerator(int priority) : base(priority){}

        public override void OnGenerate(Chunk chunk, int x, int y){
            chunk.terrain[x,y].SetTileData(TileData.GRASS);
        }

    }

}
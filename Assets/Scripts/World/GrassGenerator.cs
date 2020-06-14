using Tiles;

namespace World
{
    public class GrassGenerator : GeneratorModifier {
        TileData tileData;

        public GrassGenerator(int priority, TileData tileData) : base(priority) {
            this.tileData = tileData;
        }

        public override void BeforeGenerate(Chunk chunk){
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    chunk.terrain[x,y].SetTileData(tileData);
                }
            }
        }

    }
}
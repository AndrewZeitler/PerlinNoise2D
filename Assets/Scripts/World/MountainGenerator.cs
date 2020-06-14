using Tiles;

namespace World {

    public class MountainGenerator : GeneratorModifier {

        public MountainGenerator(int priority) : base(priority) {}

        public override void OnGenerate(Chunk chunk){
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    if(chunk.heightMap[x,y] < Biome.MOUNTAIN.MinHeight + 0.05){
                        chunk.terrain[x,y].SetTileData(TileData.DIRT);
                    } else {
                        chunk.terrain[x,y].SetTileData(TileData.STONE);
                    }
                }
            }
        }

    }

}
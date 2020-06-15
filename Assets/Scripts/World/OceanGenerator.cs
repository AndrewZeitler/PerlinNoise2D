using Tiles;

namespace World {

    public class OceanGenerator : GeneratorModifier {

        public OceanGenerator(int priority) : base(priority) {}

        public override void OnGenerate(Chunk chunk){
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    if(chunk.heightMap[x, y] > Biome.OCEAN.MaxHeight){
                        chunk.terrain[x, y].SetTileData(TileData.GRASS);
                    } else if(chunk.heightMap[x, y] > Biome.OCEAN.MaxHeight - 0.025){
                        chunk.terrain[x, y].SetTileData(TileData.SAND);
                    } else {
                        chunk.terrain[x, y].SetTileData(TileData.WATER);
                    }
                }
            }
        }

    }

}
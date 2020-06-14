using Tiles;

namespace World {

    public class GrasslandsGenerator : GeneratorModifier {

        public GrasslandsGenerator(int priority) : base(priority){}

        public override void OnGenerate(Chunk chunk){
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    if(chunk.heightMap[x,y] <= Biome.GRASSLANDS.MinHeight) {
                        chunk.terrain[x,y].SetTileData(TileData.WATER);
                    } else if(chunk.heightMap[x,y] >= Biome.GRASSLANDS.MaxHeight - 0.05){
                        chunk.terrain[x,y].SetTileData(TileData.DIRT);
                    } else {
                        chunk.terrain[x,y].SetTileData(TileData.GRASS);
                    }
                }
            }
        }

    }

}
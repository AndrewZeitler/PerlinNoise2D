using Tiles;

namespace World {

    public class GrasslandsGenerator : GeneratorModifier {

        public GrasslandsGenerator(int priority) : base(priority){}

        public override void OnGenerate(Chunk chunk, int x, int y){
            if(chunk.heightMap[x, y] > Biome.GRASSLANDS.Height.max - 0.05){
                chunk.terrain[x,y].SetTileData(TileData.DIRT);
            } else {
                chunk.terrain[x,y].SetTileData(TileData.GRASS);
            }
        }

    }

}
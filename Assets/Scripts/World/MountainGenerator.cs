using Tiles;

namespace World {

    public class MountainGenerator : GeneratorModifier {

        public MountainGenerator(int priority) : base(priority) {}

        public override void OnGenerate(Chunk chunk, int x, int y){
            if(chunk.heightMap[x,y] < Biome.MOUNTAIN.Height.min + 0.1f){
                chunk.terrain[x,y].SetTileData(TileData.DIRT);
            } else {
                chunk.terrain[x,y].SetTileData(TileData.STONE);
            }
        }

    }

}
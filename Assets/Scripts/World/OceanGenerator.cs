using Tiles;

namespace World {

    public class OceanGenerator : GeneratorModifier {

        public OceanGenerator(int priority) : base(priority) {}

        public override void OnGenerate(Chunk chunk, int x, int y){
            if(chunk.heightMap[x, y] > Biome.OCEAN.Height.max - 0.025){
                chunk.terrain[x, y].SetTileData(TileData.SAND);
            } else {
                chunk.terrain[x, y].SetTileData(TileData.WATER);
            }
        }

    }

}
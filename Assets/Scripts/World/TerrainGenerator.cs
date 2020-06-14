using Tiles;

namespace World {

    public class TerrainGenerator : GeneratorModifier {

        TileData tileData;
        PerlinNoiseGenerator generator;
        float spawnChance;

        public TerrainGenerator(int priority, TileData tileData, float spawnChance) : base(priority) {
            this.tileData = tileData;
            generator = new PerlinNoiseGenerator();
            this.spawnChance = spawnChance;
        }

        public override void OnGenerate(Chunk chunk) {
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    // Check to see if the priority is already above current tile
                    //if(chunk.terrain[x,y].tileData != TileData.AIR) continue;
                    //if(NeighbourIsAnimation(chunks, xChunk, yChunk, x, y, tileInfo.id)) continue;
                    double rand = WorldGenerator.PerlinNoise(x + chunk.x * Chunk.chunkSize, y + chunk.y * Chunk.chunkSize, tileData.Id);
                    // if(isContinuation(chunks, xChunk, yChunk, x, y, tileInfo.id)){
                    //     if(rand <= tileInfo.spawnChance + tileInfo.continuationBias){
                    //         chunk.terrain[x,y].SetTileData(TileData.nameToData[tileInfo.tileName]);
                    //     } else {
                    //         chunk.terrain[x,y].SetTileData(TileData.nameToData[tileManager.defaultTileName]);
                    //     }
                    // } else {
                        // Otherwise check if perlin noise value is less than the spawn chance of the tile
                        if(rand <= spawnChance){
                            chunk.terrain[x,y].SetTileData(tileData);
                        }
                    //}
                }
            }
        }

    }

}
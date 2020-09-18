using World.Biomes;
using System.Collections.Generic;

namespace World.Dimensions {

    public abstract class Dimension {
        public string Name { get; protected set; }
        public List<Biome> Biomes { get; protected set; }
        public Biome DefaultBiome { get; protected set; }

        public Dimension(string name){ Name = name; Biomes = new List<Biome>(); }
        
        public virtual void AddBiome(Biome biome) { 
            Biomes.Add(biome); 
            Biomes.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        }
        public virtual void GenerateChunk(Chunk chunk){
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    bool generated = false;
                    foreach(Biome biome in Biomes){
                        if(biome.CanSpawn(chunk, x, y)) {
                            biome.GenerateTile(chunk, x, y);
                            generated = true;
                        }
                    }
                    if(!generated && DefaultBiome != null) DefaultBiome.GenerateTile(chunk, x, y);
                }
            }
        }

        public virtual IEnumerator<Chunk> GenerateChunkParallel(Chunk chunk){
            chunk.chunkState = ChunkState.Generating;
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    bool generated = false;
                    foreach(Biome biome in Biomes){
                        if(biome.CanSpawn(chunk, x, y)) {
                            biome.GenerateTile(chunk, x, y);
                            generated = true;
                        }
                    }
                    if(!generated && DefaultBiome != null) DefaultBiome.GenerateTile(chunk, x, y);
                    yield return null;
                }
            }
            chunk.chunkState = ChunkState.Generated;
        }
    }

}
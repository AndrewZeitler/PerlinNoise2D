using Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World {

    public class WorldGenerator {

        public static readonly int seed = Mathf.Abs(System.Environment.TickCount / 10);
        public static readonly PerlinNoiseGenerator generator = new PerlinNoiseGenerator();

        public WorldGenerator(){
            //modifiers.Sort(GeneratorModifier.Compare);
            Random.InitState(seed);
        }

        public static double PerlinNoise(double x, double y, int id){
            return generator.perlin((x + WorldGenerator.seed * id) / 16, (y + WorldGenerator.seed * id) / 16, WorldGenerator.seed / 10000000);
        }

        public static double PerlinNoise(double x, double y){
            return generator.perlin((x + WorldGenerator.seed) / 64, (y + WorldGenerator.seed) / 64, WorldGenerator.seed / 10000000);
        }

        public void LoadChunks(Chunk[,] chunks){
            for(int x = 0; x < chunks.GetLength(0); ++x){
                for(int y = 0; y < chunks.GetLength(1); ++y){
                    //LogPerlin(chunks[x, y]);
                    GenerateHeight(chunks[x, y]);
                    chunks[x,y].biome = Biome.GetBiome(chunks[x,y]);
                }
            }

            for(int x = 0; x < chunks.GetLength(0); ++x){
                for(int y = 0; y < chunks.GetLength(1); ++y){
                    if(chunks[x,y].chunkState == ChunkState.NotGenerated){
                        for(int xc = 0; xc < Chunk.chunkSize; ++xc){
                            for(int yc = 0; yc < Chunk.chunkSize; ++yc){
                                chunks[x,y].terrain[xc,yc] = new Tile(TileData.AIR);
                                chunks[x,y].tiles[xc,yc] = new Tile(TileData.AIR);
                            }
                        }
                    }
                }
            }
            for(int x = 1; x < chunks.GetLength(0) - 1; ++x){
                for(int y = 1; y < chunks.GetLength(1) - 1; ++y){
                    if(chunks[x,y].chunkState == ChunkState.NotGenerated){
                        Biome.SmoothChunk(chunks, x, y);
                    }
                }
            }
            for(int x = 1; x < chunks.GetLength(0) - 1; ++x){
                for(int y = 1; y < chunks.GetLength(1) - 1; ++y){
                    if(chunks[x,y].chunkState == ChunkState.NotGenerated){
                        chunks[x,y].biome.GenerateChunk(chunks, x, y);
                        chunks[x,y].chunkState = ChunkState.Generated;
                    }
                }
            }
            for(int x = 2; x < chunks.GetLength(0) - 2; ++x){
                for(int y = 2; y < chunks.GetLength(1) - 2; ++y){
                    if(chunks[x,y].chunkState == ChunkState.Generated){
                        MakeObjects(chunks[x,y]);
                        chunks[x,y].chunkState = ChunkState.Rendered;
                    }
                }
            }
        }

        void MakeObjects(Chunk chunk){
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    chunk.terrain[x, y].CreateTileObject(new Vector2(x + chunk.x * Chunk.chunkSize, (y + chunk.y * Chunk.chunkSize)));
                    chunk.tiles[x, y].CreateTileObject(new Vector2(x + chunk.x * Chunk.chunkSize, (y + chunk.y * Chunk.chunkSize)));
                }
            }
        }

        void GenerateHeight(Chunk chunk){
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    chunk.heightMap[x, y] = WorldGenerator.PerlinNoise(x + chunk.x * Chunk.chunkSize, y + chunk.y * Chunk.chunkSize);
                }
            }
        }

        void LogPerlin(Chunk chunk){
            Debug.Log(new Vector2(chunk.x, chunk.y));
            for(int x = 0; x < Chunk.chunkSize; ++x){
                string line = "";
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    line += " " + WorldGenerator.PerlinNoise(x + chunk.x * Chunk.chunkSize, y + chunk.y * Chunk.chunkSize);
                }
                Debug.Log(line);
            }
        }
    }

}
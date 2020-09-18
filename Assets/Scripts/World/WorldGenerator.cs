using Tiles;
using Utils;
using UnityEngine;
using World.Dimensions;
using System.Collections.Generic;

namespace World {

    public class WorldGenerator {

        // public static readonly int seed = Mathf.Abs(System.Environment.TickCount / 10);
        public int seed;
        public static readonly PerlinNoiseGenerator generator = new PerlinNoiseGenerator();
        PerlinGenerator perlinGenerator;

        public WorldGenerator(int seed = -1){
            this.seed = (seed != -1 ? seed : System.Environment.TickCount);
            Debug.Log("Seed: " + seed);
            perlinGenerator = new PerlinGenerator(0.05f, 0, seed);

            DimensionRegistry.RegisterDimension(new SurfaceDimension(seed));
            World.WorldManager.SetDimension("Surface");
        }

        // public static double PerlinNoise(double x, double y, int id){
        //     return generator.perlin((x + WorldGenerator.seed * id) / 16, (y + WorldGenerator.seed * id) / 16, WorldGenerator.seed / 10000000);
        // }

        // public static double PerlinNoise(double x, double y){
        //     return generator.perlin((x + WorldGenerator.seed) / 64, (y + WorldGenerator.seed) / 64, WorldGenerator.seed / 10000000);
        // }

        public void LoadChunks(Chunk[,] chunks){
            Dimension dimension = WorldManager.Dimension;
            for(int x = 0; x < chunks.GetLength(0); ++x){
                for(int y = 0; y < chunks.GetLength(1); ++y){
                    //LogPerlin(chunks[x, y]);
                    GenerateHeight(chunks[x, y]);
                    //chunks[x,y].biome = Biome.GetBiome(chunks[x,y]);
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
                        SceneManager.sceneManager.StartCoroutine(dimension.GenerateChunkParallel(chunks[x,y]));
                    }
                }
            }
            for(int x = 2; x < chunks.GetLength(0) - 2; ++x){
                for(int y = 2; y < chunks.GetLength(1) - 2; ++y){
                    if(chunks[x,y].chunkState == ChunkState.Generated){
                        SceneManager.sceneManager.StartCoroutine(MakeObjects(chunks[x,y]));
                    }
                }
            }
        }

        IEnumerator<Chunk> MakeObjects(Chunk chunk){
            chunk.chunkState = ChunkState.Rendering;
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    chunk.terrain[x, y].CreateTileObject(new Vector2(x + chunk.x * Chunk.chunkSize, (y + chunk.y * Chunk.chunkSize)));
                    chunk.tiles[x, y].CreateTileObject(new Vector2(x + chunk.x * Chunk.chunkSize, (y + chunk.y * Chunk.chunkSize)));
                    yield return null;
                }
            }
            chunk.chunkState = ChunkState.Rendered;
        }

        void GenerateHeight(Chunk chunk){
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    chunk.heightMap[x, y] = perlinGenerator.Perlin(x + chunk.x * Chunk.chunkSize, y + chunk.y * Chunk.chunkSize);
                }
            }
        }

        void LogPerlin(Chunk chunk){
            Debug.Log(new Vector2(chunk.x, chunk.y));
            for(int x = 0; x < Chunk.chunkSize; ++x){
                string line = "";
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    line += " " + perlinGenerator.Perlin(x + chunk.x * Chunk.chunkSize, y + chunk.y * Chunk.chunkSize);
                }
                Debug.Log(line);
            }
        }
    }

}
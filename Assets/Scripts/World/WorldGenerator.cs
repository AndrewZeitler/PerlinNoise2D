using Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World {

    public class WorldGenerator {
        // public static readonly List<GeneratorModifier> modifiers = new List<GeneratorModifier> {
        //     new GrassGenerator(0, TileData.GRASS),
        //     new TerrainGenerator(0, TileData.DIRT, 0.45f),
        //     new TerrainGenerator(1, TileData.STONE, 0.25f),
        //     new TerrainGenerator(2, TileData.WATER, 0.3f),
        //     new ResourceGenerator(0, TileData.DIRT_ROCK, 0.025f, 0, new TileData[]{TileData.DIRT}),
        //     new ResourceGenerator(0, TileData.STONE_ROCK, 0.025f, 0, new TileData[]{TileData.STONE}),
        //     new ResourceGenerator(0, TileData.LILYPAD, 0.025f, 0, new TileData[]{TileData.WATER}),
        //     new ResourceGenerator(1, TileData.TREE, 0.025f, 0.27f, new TileData[]{TileData.GRASS})
        // };

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
            return generator.perlin((x + WorldGenerator.seed) / 32, (y + WorldGenerator.seed) / 32, WorldGenerator.seed / 10000000);
        }

        public void LoadChunks(Chunk[,] chunks){
            for(int x = 0; x < chunks.GetLength(0); ++x){
                for(int y = 0; y < chunks.GetLength(1); ++y){
                    LogPerlin(chunks[x, y]);
                    GenerateHeight(chunks[x, y]);
                    chunks[x,y].biome = GetBiome(chunks[x,y]);
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
            for(int x = 0; x < chunks.GetLength(0); ++x){
                for(int y = 0; y < chunks.GetLength(1); ++y){
                    if(chunks[x,y].chunkState == ChunkState.NotGenerated){
                        foreach(GeneratorModifier modifier in chunks[x,y].biome.Modifiers){
                            modifier.BeforeGenerate(chunks[x, y]);
                        }
                    }
                }
            }
            for(int x = 0; x < chunks.GetLength(0); ++x){
                for(int y = 0; y < chunks.GetLength(1); ++y){
                    if(chunks[x,y].chunkState == ChunkState.NotGenerated){
                        foreach(GeneratorModifier modifier in chunks[x,y].biome.Modifiers){
                            modifier.OnGenerate(chunks[x, y]);
                        }
                    }
                }
            }
            for(int x = 0; x < chunks.GetLength(0); ++x){
                for(int y = 0; y < chunks.GetLength(1); ++y){
                    if(chunks[x,y].chunkState == ChunkState.NotGenerated){
                        foreach(GeneratorModifier modifier in chunks[x,y].biome.Modifiers){
                            modifier.AfterGenerate(chunks[x, y]);
                            chunks[x,y].chunkState = ChunkState.Generated;
                        }
                    }
                }
            }
            for(int x = 1; x < chunks.GetLength(0) - 1; ++x){
                for(int y = 1; y < chunks.GetLength(1) - 1; ++y){
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

        public static Biome GetBiome(Chunk chunk){
            double averageHeight = 0;
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    averageHeight += chunk.heightMap[x,y];
                }
            }
            averageHeight /= Chunk.chunkSize * Chunk.chunkSize;

            foreach(Biome biome in Biome.BIOMES){
                if(averageHeight > biome.MinHeight && averageHeight <= biome.MaxHeight){
                    return biome;
                }
            }
            return null;;
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
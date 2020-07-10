using System.Collections.Generic;
using Tiles;
using UnityEngine;
using Utils;

namespace World {

    public enum BiomeType {
        GRASSLANDS,
        OCEAN,
        MOUNTAIN,
        DEADLANDS,
        FOREST
    }

    public class Biome : BiomeEnum {
        public static readonly float chunkBlur = 0.75f;  
        public static List<Biome> BIOMES = new List<Biome>();

        public static readonly Biome GRASSLANDS = new Biome(BiomeType.GRASSLANDS, new Range(0.40f, 0.70f), new List<GeneratorModifier>(){
            new GrasslandsGenerator(1),
            new TreeGenerator(1, TileData.TREE, 0.025f, new Vector2(0, 0), 0, new TileData[]{TileData.GRASS})
        });
        public static readonly Biome FOREST = new Biome(BiomeType.FOREST, new Range(0.425f, 0.55f), new List<GeneratorModifier>(){
            new ForestGenerator(1),
            new TreeGenerator(1, TileData.TREE, 0.025f, new Vector2(0.45f, 0.525f), 0.45f, new TileData[]{TileData.GRASS})
        });
        public static readonly Biome OCEAN = new Biome(BiomeType.OCEAN, new Range(0, 0.45f), new List<GeneratorModifier>(){
            new OceanGenerator(1),
            new VariationGenerator(0, TileData.LILYPAD, 0.025f, new TileData[]{TileData.WATER})
        });
        public static readonly Biome MOUNTAIN = new Biome(BiomeType.MOUNTAIN, new Range(0.70f, 1), new List<GeneratorModifier>(){
            new MountainGenerator(1),
            new VariationGenerator(0, TileData.DIRT_ROCK, 0.025f, new TileData[]{TileData.DIRT}),
            new VariationGenerator(0, TileData.STONE_ROCK, 0.025f, new TileData[]{TileData.STONE})
        });
        // public static readonly Biome GRASSLANDS = new Biome(BiomeType.GRASSLANDS, new Range(0, 0.5f), new List<GeneratorModifier>(){
        //     new SolidGenerator(1, TileData.GRASS)
        // });
        // public static readonly Biome DEADLANDS = new Biome(BiomeType.DEADLANDS, new Range(0.5f, 1), new List<GeneratorModifier>(){
        //     new SolidGenerator(1, TileData.DEADGRASS)
        // });

        public Biome(BiomeType type, Range height, List<GeneratorModifier> modifiers) : base(type, height, modifiers) {
            BIOMES.Add(this);
        }

        public void GenerateChunk(Chunk[,] chunks, int xChunk, int yChunk){
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    //BlurChunk(chunks, xChunk, yChunk, x, y);
                    BlurChunk(chunks, xChunk, yChunk, x, y);
                }
            }
        }

        // private void BlurChunk(Chunk[,] chunks, int xChunk, int yChunk, int x, int y){
        //     BiomeType biome = chunks[xChunk, yChunk].biome.Type;
        //     if(chunks[xChunk, yChunk + 1].biome.Type != biome){
        //         double top = chunks[xChunk, yChunk].averageHeight;
        //         double bottom = chunks[xChunk, yChunk + 1].averageHeight;
        //         if(top > bottom){
        //             if(top - (top - bottom) / 2 > chunks[xChunk, yChunk].heightMap[x, y]){
        //                 chunks[xChunk, yChunk + 1].biome.GenerateTile(chunks, xChunk, yChunk, x, y);
        //                 return;
        //             }
        //         } else {
        //             if(top - (top - bottom) / 2 < chunks[xChunk, yChunk].heightMap[x, y]){
        //                 chunks[xChunk, yChunk + 1].biome.GenerateTile(chunks, xChunk, yChunk, x, y);
        //                 return;
        //             }
        //         }
                
        //     } else if(chunks[xChunk + 1, yChunk].biome.Type != biome){
        //         double left = chunks[xChunk, yChunk].averageHeight;
        //         double right = chunks[xChunk + 1, yChunk].averageHeight;
        //         if(left > right){
        //             if(left - (left - right) / 2 > chunks[xChunk, yChunk].heightMap[x, y]){
        //                 chunks[xChunk + 1, yChunk].biome.GenerateTile(chunks, xChunk, yChunk, x, y);
        //                 return;
        //             }
        //         } else {
        //             if(left - (left - right) / 2 < chunks[xChunk, yChunk].heightMap[x, y]){
        //                 chunks[xChunk + 1, yChunk].biome.GenerateTile(chunks, xChunk, yChunk, x, y);
        //                 return;
        //             }
        //         }
        //     }
        //     GenerateTile(chunks, xChunk, yChunk, x, y);
        // }

        // private void BlurChunk(Chunk[,] chunks, int xChunk, int yChunk, int x, int y){
        //     List<Chunk> others = new List<Chunk>();
        //     Chunk chunk = chunks[xChunk, yChunk];
        //     if(chunks[xChunk + 1, yChunk].biome.Type != chunk.biome.Type) others.Add(chunks[xChunk + 1, yChunk]);
        //     if(chunks[xChunk - 1, yChunk + 1].biome.Type != chunk.biome.Type) others.Add(chunks[xChunk - 1, yChunk + 1]);
        //     if(chunks[xChunk, yChunk + 1].biome.Type != chunk.biome.Type) others.Add(chunks[xChunk, yChunk + 1]);
        //     if(chunks[xChunk + 1, yChunk + 1].biome.Type != chunk.biome.Type) others.Add(chunks[xChunk + 1, yChunk + 1]);
        //     double highest = chunk.averageHeight;
        //     double lowest = chunk.averageHeight;
        //     Biome biome = null;
        //     foreach(Chunk other in others){
        //         if(other.averageHeight > chunk.averageHeight && other.averageHeight > highest){
        //             if(chunk.heightMap[x,y] > (other.averageHeight + chunk.averageHeight) / 2) {
        //                 highest = other.averageHeight;
        //                 biome = other.biome;
        //             }
        //         } else if(other.averageHeight < chunk.averageHeight && other.averageHeight < lowest){
        //             if(chunk.heightMap[x,y] < (other.averageHeight + chunk.averageHeight) / 2) {
        //                 lowest = other.averageHeight;
        //                 biome = other.biome;
        //             }
        //         }
        //     }
        //     if(biome != null){
        //         biome.GenerateTile(chunks, xChunk, yChunk, x, y);
        //     } else {
        //         GenerateTile(chunks, xChunk, yChunk, x, y);
        //     }
        // }

        private void BlurChunk(Chunk[,] chunks, int xChunk, int yChunk, int x, int y){
            Chunk chunk = chunks[xChunk, yChunk];
            double highest = chunk.averageHeight;
            double lowest = chunk.averageHeight;
            Biome biome = null;
            for(int xd = -1; xd < 2; ++xd){
                for(int yd = -1; yd < 2; ++yd){
                    Chunk other = chunks[xChunk + xd, yChunk + yd];
                    if(other.biome.Type == chunk.biome.Type) continue;
                    if(other.averageHeight > chunk.averageHeight && other.averageHeight > highest){
                        if(chunk.heightMap[x,y] > (other.averageHeight + chunk.averageHeight) / 2) {
                            highest = other.averageHeight;
                            biome = other.biome;
                        }
                    } else if(other.averageHeight < chunk.averageHeight && other.averageHeight < lowest){
                        if(chunk.heightMap[x,y] < (other.averageHeight + chunk.averageHeight) / 2) {
                            lowest = other.averageHeight;
                            biome = other.biome;
                        }
                    }
                }
            }
            if(biome != null){
                biome.GenerateTile(chunks, xChunk, yChunk, x, y);
            } else {
                GenerateTile(chunks, xChunk, yChunk, x, y);
            }
        }

        public void GenerateTile(Chunk[,] chunks, int xChunk, int yChunk, int x, int y){
            Chunk chunk = chunks[xChunk, yChunk];
            foreach(GeneratorModifier modifier in Modifiers){
                modifier.BeforeGenerate(chunk, x, y);
            }

            foreach(GeneratorModifier modifier in Modifiers){
                modifier.OnGenerate(chunk, x, y);
            }

            foreach(GeneratorModifier modifier in Modifiers){
                modifier.AfterGenerate(chunk, x, y);
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
            chunk.averageHeight = averageHeight;

            Biome result = null;
            double max = 0;
            foreach(Biome biome in Biome.BIOMES){
                if(biome.Height.IsInRange(averageHeight)){
                    double dev = biome.Height.GetDeviance(averageHeight);
                    if(dev > max) {
                        max = dev;
                        result = biome;
                    }
                }
            }
            
            return result;
        }

        public static void SmoothChunk(Chunk[,] chunks, int xChunk, int yChunk){
            if(chunks[xChunk, yChunk].biome.Type != chunks[xChunk - 1, yChunk].biome.Type && 
                    chunks[xChunk - 1, yChunk].biome.Type == chunks[xChunk + 1, yChunk].biome.Type){
                chunks[xChunk, yChunk].biome = chunks[xChunk - 1, yChunk].biome;
            } else if(chunks[xChunk, yChunk].biome.Type != chunks[xChunk, yChunk - 1].biome.Type && 
                    chunks[xChunk, yChunk - 1].biome.Type == chunks[xChunk, yChunk + 1].biome.Type){
                chunks[xChunk, yChunk].biome = chunks[xChunk, yChunk - 1].biome;
            }
        }
    }

}

// namespace World {
//     public enum BiomeType {
//         GRASSLANDS,
//         OCEAN,
//         MOUNTAIN,
//         FOREST
//     }

//     public abstract class Biome {
//         public BiomeType biome { get; }
//         public Vector2 height { get; }
//         public GeneratorModifier[] modifiers { get; }

//         public void GenerateChunk(Chunk chunk);

//         public void GenerateTile(Chunk chunk, int x, int y);
//     }
// }
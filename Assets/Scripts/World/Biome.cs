using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace World {

    public enum BiomeType {
        GRASSLANDS,
        OCEAN,
        MOUNTAIN
    }

    public class Biome : BiomeEnum {
        public static List<Biome> BIOMES = new List<Biome>();

        public static readonly Biome GRASSLANDS = new Biome(BiomeType.GRASSLANDS, 0.40, 0.70, new List<GeneratorModifier>(){
            new GrasslandsGenerator(1),
            new TreeGenerator(1, TileData.TREE, 0.025f, new Vector2(0.45f, 0.525f), 0.35f, new TileData[]{TileData.GRASS})
        });
        public static readonly Biome OCEAN = new Biome(BiomeType.OCEAN, 0, 0.40, new List<GeneratorModifier>(){
            new OceanGenerator(1),
            new VariationGenerator(0, TileData.LILYPAD, 0.025f, new TileData[]{TileData.WATER})
        });
        public static readonly Biome MOUNTAIN = new Biome(BiomeType.MOUNTAIN, 0.70, 1, new List<GeneratorModifier>(){
            new MountainGenerator(1),
            new VariationGenerator(0, TileData.DIRT_ROCK, 0.025f, new TileData[]{TileData.DIRT}),
            new VariationGenerator(0, TileData.STONE_ROCK, 0.025f, new TileData[]{TileData.STONE})
        });

        public Biome(BiomeType type, double minHeight, double maxHeight, List<GeneratorModifier> modifiers) : base(type, minHeight, maxHeight, modifiers) {
            BIOMES.Add(this);
        }
    }

}
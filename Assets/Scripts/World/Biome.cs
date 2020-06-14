using System.Collections.Generic;
using UnityEngine;

namespace World {

    public enum BiomeType {
        GRASSLANDS,
        OCEAN,
        MOUNTAIN
    }

    public class Biome : BiomeEnum {
        public static List<Biome> BIOMES = new List<Biome>();

        public static readonly Biome GRASSLANDS = new Biome(BiomeType.GRASSLANDS, 0.35, 0.65, new List<GeneratorModifier>(){new GrasslandsGenerator(1)});
        public static readonly Biome OCEAN = new Biome(BiomeType.OCEAN, 0, 0.35, new List<GeneratorModifier>(){new OceanGenerator(1)});
        public static readonly Biome MOUNTAIN = new Biome(BiomeType.MOUNTAIN, 0.65, 1, new List<GeneratorModifier>(){new MountainGenerator(1)});

        public Biome(BiomeType type, double minHeight, double maxHeight, List<GeneratorModifier> modifiers) : base(type, minHeight, maxHeight, modifiers) {
            Debug.Log(BIOMES);
            BIOMES.Add(this);
        }
    }

}
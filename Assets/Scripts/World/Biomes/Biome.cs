using Utils;
using System.Collections.Generic;
using World.Biomes.Modifiers;

namespace World.Biomes {

    public abstract class Biome {
        public PerlinGenerator perlinGenerator { get; protected set; }
        public string Name { get; protected set; }
        public int Priority { get; protected set; }
        public List<BiomeModifier> Modifiers { get; }


        public Biome(string name, int priority) { 
            Name = name;
            Priority = priority;
            Modifiers = new List<BiomeModifier>();
        }

        public Biome(string name, int priority, float perlinZoom, float spawnChance, int seed) { 
            Name = name;
            Priority = priority;
            Modifiers = new List<BiomeModifier>();
            perlinGenerator = new PerlinGenerator(perlinZoom, spawnChance, seed);
        }

        public virtual void GenerateTile(Chunk chunk, int x, int y){
            foreach(BiomeModifier modifier in Modifiers){
                modifier.BeforeGenerate(chunk, x, y);
            }
            foreach(BiomeModifier modifier in Modifiers){
                modifier.OnGenerate(chunk, x, y);
            }
            foreach(BiomeModifier modifier in Modifiers){
                modifier.AfterGenerate(chunk, x, y);
            }
        }

        public virtual bool CanSpawn(Chunk chunk, int x, int y) {
            if(perlinGenerator == null) return false;
            return perlinGenerator.CanSpawn(x + chunk.x * Chunk.chunkSize, y + chunk.y * Chunk.chunkSize);
        }

        public void AddModifier(BiomeModifier modifier){ 
            Modifiers.Add(modifier);
            Modifiers.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        }
    }

}
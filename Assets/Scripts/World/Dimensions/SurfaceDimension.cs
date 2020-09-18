using World.Biomes;

namespace World.Dimensions
{
    
    public class SurfaceDimension : Dimension {

        public SurfaceDimension(int seed) : base("Surface") {
            DefaultBiome = new OceanBiome(0.05f, 0.5f, seed);
            AddBiome(new GrasslandBiome(0.009f, 0.41f, seed));
            AddBiome(new DeadlandsBiome(0.009f, 0.41f, seed));
            AddBiome(new DesertBiome(0.009f, 0.41f, seed));
            AddBiome(new ForestBiome(0.009f, 0.41f, seed));
        }

    }

}
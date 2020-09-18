namespace Utils {

    public class PerlinGenerator {
        public static int seedVariation = 0;
        float perlinZoom;
        float spawnChance;
        int seed;
        float offsetMaxX;
        float offsetMaxY;

        public PerlinGenerator(float perlinZoom, float spawnChance, int seed){
            this.perlinZoom = perlinZoom;
            this.spawnChance = spawnChance;
            this.seed = seed;
            // UnityEngine.Random.InitState(seed + seedVariation);
            // ++seedVariation;
            offsetMaxX = UnityEngine.Random.Range(0f, 10000f);
            offsetMaxY = UnityEngine.Random.Range(0f, 10000f);
        }

        public bool CanSpawn(int x, int y){
            return (Perlin(x, y) / 2f < spawnChance);
        }

        public float Perlin(int x, int y){
            return UnityEngine.Mathf.PerlinNoise(offsetMaxX + x * perlinZoom, offsetMaxY + y * perlinZoom) + 
                    UnityEngine.Mathf.PerlinNoise(offsetMaxX - x * perlinZoom, offsetMaxY - y * perlinZoom);
        }
    }

}
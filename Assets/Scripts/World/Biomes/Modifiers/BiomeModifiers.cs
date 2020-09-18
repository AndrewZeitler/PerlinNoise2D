
namespace World.Biomes.Modifiers {

    public abstract class BiomeModifier {

        public int Priority { get; private set; }

        public BiomeModifier(int priority) { this.Priority = Priority; }

        public virtual void BeforeGenerate(Chunk chunk, int x, int y) {}
        public virtual void OnGenerate(Chunk chunk, int x, int y) {}
        public virtual void AfterGenerate(Chunk chunk, int x, int y) {}

        public static int Compare(GeneratorModifier obj1, GeneratorModifier obj2){
            return obj1.priority.CompareTo(obj2.priority);
        }

    }

}
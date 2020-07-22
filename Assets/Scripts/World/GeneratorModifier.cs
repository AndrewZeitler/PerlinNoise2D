
namespace World {
    public abstract class GeneratorModifier {

        public int priority { get; private set; }

        public GeneratorModifier(int priority) { this.priority = priority; }

        public virtual void BeforeGenerate(Chunk chunk, int x, int y) {}
        public virtual void OnGenerate(Chunk chunk, int x, int y) {}
        public virtual void AfterGenerate(Chunk chunk, int x, int y) {}

        public static int Compare(GeneratorModifier obj1, GeneratorModifier obj2){
            return obj1.priority.CompareTo(obj2.priority);
        }

    }

}
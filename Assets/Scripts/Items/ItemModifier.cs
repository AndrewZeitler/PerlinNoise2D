
namespace Items {
    public abstract class ItemModifier {
        public Item item;

        public abstract void Initialize(Item item);

        public ItemModifier Clone(){
            return this.MemberwiseClone() as ItemModifier;
        }

        public virtual void Destroy() { item = null; }
    }
}
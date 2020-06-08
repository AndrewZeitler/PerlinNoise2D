using System.Collections;
using System.Collections.Generic;

namespace Items {
    public abstract class ItemEnum {
        public string Name { get; }
        public int Id { get; }
        public ItemModifier[] ItemModifiers { get; }

        protected ItemEnum(string name, int id, ItemModifier[] itemModifiers){
            Name = name;
            Id = id;
            ItemModifiers = itemModifiers;
        }

        public override string ToString() { return Name; }
        public override bool Equals(object obj) {
            var other = obj as ItemEnum;
            if(other == null) return false;
            if(!GetType().Equals(obj.GetType())) return false;
            return (Name.Equals(other.Name));
        }

        public override int GetHashCode(){
            return Name.GetHashCode();
        }
    }
}
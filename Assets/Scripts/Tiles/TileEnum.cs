using System.Collections;
using System.Collections.Generic;

namespace Tiles {
    public abstract class TileEnum {
        public string Name { get; }
        public int Id { get; }
        public TileModifier[] TileModifiers { get; }
        public bool IsWalkable { get; }

        protected TileEnum(string name, int id, TileModifier[] tileModifiers, bool isWalkable){
            Name = name;
            Id = id;
            TileModifiers = tileModifiers;
            IsWalkable = isWalkable;
        }

        public override string ToString() { return Name; }
        public override bool Equals(object obj) {
            var other = obj as TileEnum;
            if(other == null) return false;
            if(!GetType().Equals(obj.GetType())) return false;
            return (Id == other.Id);
        }

        public override int GetHashCode(){
            return Id.GetHashCode();
        }
    }
}
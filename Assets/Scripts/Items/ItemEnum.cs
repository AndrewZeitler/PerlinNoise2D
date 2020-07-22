using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    public abstract class ItemEnum {
        public string Name { get; }
        public int Id { get; }
        public ItemModifier[] ItemModifiers { get; }
        public Sprite ItemSprite { get; }

        protected ItemEnum(string name, int id, ItemModifier[] itemModifiers){
            Name = name;
            Id = id;
            ItemModifiers = itemModifiers;
            ItemSprite = SpriteManager.GetItemSprite(Name);
        }

        public override string ToString() { return Name; }
        public override bool Equals(object obj) {
            var other = obj as ItemEnum;
            if(other == null) return false;
            if(!GetType().Equals(obj.GetType())) return false;
            return (Name.Equals(other.Name));
        }

        public static int Compare(ItemEnum obj, ItemEnum other){
            return obj.Id.CompareTo(other.Id);
        }

        public override int GetHashCode(){
            return Name.GetHashCode();
        }
    }
}
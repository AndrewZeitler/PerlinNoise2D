using System.Collections;
using System.Collections.Generic;

namespace World {
    public abstract class BiomeEnum {
        public BiomeType Type { get; }
        public double MinHeight { get; }
        public double MaxHeight { get; }
        public List<GeneratorModifier> Modifiers;

        protected BiomeEnum(BiomeType type, double minHeight, double maxHeight, List<GeneratorModifier> modifiers){
            Type = type;
            MinHeight = minHeight;
            MaxHeight = maxHeight;
            Modifiers = modifiers;
            Modifiers.Sort(GeneratorModifier.Compare);
        }

        public override string ToString() { return Type.ToString(""); }
        public override bool Equals(object obj) {
            var other = obj as BiomeEnum;
            if(other == null) return false;
            if(!GetType().Equals(obj.GetType())) return false;
            return (Type.Equals(other.Type));
        }

        public override int GetHashCode(){
            return Type.GetHashCode();
        }
    }
}
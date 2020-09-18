// using System.Collections.Generic;
// using Utils;

// namespace World {
//     public abstract class BiomeEnum {
//         public BiomeType Type { get; }
//         public Range Height { get; }
//         public List<GeneratorModifier> Modifiers;

//         protected BiomeEnum(BiomeType type, Range height, List<GeneratorModifier> modifiers){
//             Type = type;
//             Height = height;
//             Modifiers = modifiers;
//             Modifiers.Sort(GeneratorModifier.Compare);
//         }

//         public override string ToString() { return Type.ToString(""); }
//         public override bool Equals(object obj) {
//             var other = obj as BiomeEnum;
//             if(other == null) return false;
//             if(!GetType().Equals(obj.GetType())) return false;
//             return (Type.Equals(other.Type));
//         }

//         public override int GetHashCode(){
//             return Type.GetHashCode();
//         }
//     }
// }
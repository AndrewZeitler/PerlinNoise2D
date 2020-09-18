using System.Collections.Generic;

namespace World.Dimensions {

    public static class DimensionRegistry {
        public static Dictionary<string, Dimension> Dimensions { get; } = new Dictionary<string, Dimension>();

        public static void RegisterDimension(Dimension dimension){
            if(Dimensions.ContainsKey(dimension.Name)) return;
            Dimensions[dimension.Name] = dimension;
        }

    }

}
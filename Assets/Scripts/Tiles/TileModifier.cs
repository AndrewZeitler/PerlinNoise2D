using Tiles;
using UnityEngine;

namespace Tiles {
    public abstract class TileModifier {
        public Tile tile;
        public virtual void Initialize(Tile tile) { this.tile = tile; }

        public TileModifier Clone(){
            return this.MemberwiseClone() as TileModifier;
        }

        public virtual void Destroy(){
            tile = null;
        }
    }
}
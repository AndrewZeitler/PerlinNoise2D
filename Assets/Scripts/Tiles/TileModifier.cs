using Tiles;
using UnityEngine;

public abstract class TileModifier {
    public Tile tile;
    public abstract void Initialize(Tile tile);

    public TileModifier Clone(){
        return this.MemberwiseClone() as TileModifier;
    }

    public virtual void Destroy(){
        tile = null;
    }
}
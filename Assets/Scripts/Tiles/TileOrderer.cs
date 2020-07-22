using UnityEngine;
using World;

namespace Tiles {
    public class TileOrderer : TileModifier {

        public override void Initialize(Tile tile) {
            this.tile = tile;
            tile.AddCreateListener(OnObjectCreation);
        }

        public void OnObjectCreation(){
            Vector3 pos = tile.gameObject.transform.position;
            tile.spriteRenderer.sortingLayerName = "Tiles";
            if(!tile.tileData.IsWalkable){
                tile.gameObject.transform.position = new Vector3(pos.x, pos.y, pos.z + 0.25f * (pos.x % 2 + 1));
            } else {
                tile.gameObject.transform.position = new Vector3(pos.x, pos.y, pos.z + 10);
            }
        }
    }
}
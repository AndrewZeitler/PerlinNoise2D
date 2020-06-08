using UnityEngine;
using World;

namespace Tiles {
    public class TileOrderer : TileModifier {

        public override void Initialize(Tile tile) {
            this.tile = tile;
            tile.AddCreateListener(OnObjectCreation);
        }

        public void OnObjectCreation(){
            Vector2 pos = tile.gameObject.transform.position;
            int yc = Mathf.FloorToInt(pos.y / Chunk.chunkSize);
            int y = Mathf.FloorToInt(pos.y % Chunk.chunkSize);
            if(y < 0) y += Chunk.chunkSize;
            tile.spriteRenderer.sortingLayerName = "Tiles";
            tile.gameObject.transform.position = new Vector3(pos.x, pos.y, y + yc * Chunk.chunkSize + 0.5f * (pos.x % 2));
        }
    }
}
using UnityEngine;
using World;

namespace Tiles {
    public class TileRenderer : TileModifier {

        int tileAmounts;
        Sprite sprite;

        public TileRenderer(int tileAmounts){
            this.tileAmounts = tileAmounts;
        }

        public override void Initialize(Tile tile) {
            this.tile = tile;
            tile.AddCreateListener(UpdateSprite);
        }

        public void UpdateSprite(){
            if(!sprite){
                int rand = Random.Range(0, tileAmounts - 1);
                sprite = SpriteManager.GetTileSprite(tile.tileData.Name + rand.ToString());
            }
            tile.spriteRenderer.sprite = sprite;
            if(!tile.tileData.IsWalkable) tile.gameObject.AddComponent<PolygonCollider2D>();
        }
    }
}
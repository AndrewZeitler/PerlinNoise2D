using UnityEngine;
using World;

namespace Tiles {
    public class AutoTile : TileModifier {

        public override void Initialize(Tile tile) {
            this.tile = tile;
            tile.AddCreateListener(UpdateSprite);
        }

        public void UpdateSprite(){
            Vector2 pos = tile.gameObject.transform.position;
            Tile[,] grid = WorldManager.GetTerrainGrid(new Vector2(pos.x - 1, pos.y - 1), new Vector2Int(3, 3));
            int[,] ids = new int[3, 3];
            for(int x = 0; x < 3; ++x){
                for(int y = 0; y < 3; ++y){
                    ids[x, y] = grid[x, y].tileData.Id;
                    if(x != 1 || y != 1){
                        grid[x, y].AddDataChangeListener(UpdateSprite);
                    }
                }
            }
            TerrainBrush.TileType tileType = TerrainBrush.GetTileType(ids, 1, 1);
            if(tileType == TerrainBrush.TileType.Error){
                tile.SetTileData(TileData.GRASS);
                return;
            }
            tile.spriteRenderer.sortingLayerName = "Terrain";
            tile.spriteRenderer.sprite = SpriteManager.GetTileSprite(tile.tileData.Name + tileType.ToString(""));
            if(!tile.tileData.IsWalkable) tile.gameObject.AddComponent<PolygonCollider2D>();
        }
    }
}
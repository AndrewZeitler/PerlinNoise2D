using UnityEngine;
using World;
using Utils;

namespace Tiles {
    public class AutoTile : TileModifier {
        Sprite sprite;
        TerrainBrush.TileType type;
        int tileAmounts;

        public AutoTile(int tileAmounts){
            this.tileAmounts = tileAmounts;
            type = TerrainBrush.TileType.Error;
        }

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
            if(type == tileType && tileType != TerrainBrush.TileType.Error){
                tile.spriteRenderer.sprite = sprite;
                if(!tile.tileData.IsWalkable) tile.gameObject.AddComponent<PolygonCollider2D>();
                return;
            }
            type = tileType;

            int other = TerrainBrush.GetOtherType(ids, 1, 1, tileType);
            if(tileType == TerrainBrush.TileType.Error){
                // tile.SetTileData(TileData.idToData[other]);
                // return;
                SetTileAsCenter();
            } else {
                string name = "";
                if(other != -1){
                    name = tile.tileData.Name + TileData.idToData[other].Name + (int)tileType;
                    tile.spriteRenderer.sortingLayerName = "Terrain";
                    sprite = SpriteManager.GetTileSprite(name);
                    if(sprite == null){
                        SetTileAsCenter();
                    }
                } else {
                    SetTileAsCenter();
                }
            }
            tile.spriteRenderer.sprite = sprite;
            if(!tile.tileData.IsWalkable) tile.gameObject.AddComponent<PolygonCollider2D>();
        }

        public void SetTileAsCenter(){
            if(!sprite){
                int rand = Random.Range(0, tileAmounts - 1);
                sprite = SpriteManager.GetTileSprite(tile.tileData.Name + rand.ToString());
            }
        }
    }
}
using UnityEngine;
using World;
using System.Collections;

namespace Tiles {
    public class AutoTileAnimated : TileModifier {
        public int frames;
        float frameRate;
        int currFrame;
        string name;

        public AutoTileAnimated(int frames, float frameRate){
            this.frames = frames;
            this.frameRate = frameRate;
            currFrame = 0;
        }

        public override void Initialize(Tile tile) {
            this.tile = tile;
            tile.AddCreateListener(UpdateSprite);
            tile.AddDestroyListener(StopAnimation);
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
            name = tile.tileData.Name + tileType.ToString("");
            tile.spriteRenderer.sprite = tile.tileData.GetSprite(name + currFrame.ToString());
            if(!tile.tileData.IsWalkable) tile.gameObject.AddComponent<PolygonCollider2D>();
            tile.tileScript.StartCoroutine(Animator());
        }

        public IEnumerator Animator(){
            while(tile != null){
                yield return new WaitForSeconds(frameRate);
                ChangeFrame();
            }
        }

        public void ChangeFrame(){
            ++currFrame;
            if(currFrame >= frames) currFrame = 0;
            tile.spriteRenderer.sprite = tile.tileData.GetSprite(name + currFrame.ToString());
        }

        public void StopAnimation(){
            tile.tileScript.StopCoroutine(Animator());
        }

        public override void Destroy(){
            StopAnimation();
            tile = null;
        }
    }
}
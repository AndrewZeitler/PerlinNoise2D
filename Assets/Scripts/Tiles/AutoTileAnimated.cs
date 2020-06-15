using UnityEngine;
using World;
using System.Collections;

namespace Tiles {
    public class AutoTileAnimated : TileModifier {
        public int frames;
        float frameRate;
        int currFrame;
        string name;
        int inc = 1;

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
                    // if(x != 1 || y != 1){
                    //     grid[x, y].AddDataChangeListener(UpdateSprite);
                    // }
                }
            }
            TerrainBrush.TileType tileType = TerrainBrush.GetTileType(ids, 1, 1);
            int other = TerrainBrush.GetOtherType(ids, 1, 1, tileType);
            if(tileType == TerrainBrush.TileType.Error){
                tile.SetTileData(TileData.idToData[other]);
                return;
            }
            if(other != -1){
                name = tile.tileData.Name + TileData.idToData[other].Name + (int)tileType;
            } else {
                name = tile.tileData.Name + "0";
            }
            Sprite sprite = SpriteManager.GetTileSprite(name + "_" + currFrame.ToString());
            if(sprite == null){
                name = tile.tileData.Name + "0";
                sprite = SpriteManager.GetTileSprite(name + "_" + currFrame.ToString());
            }
            tile.spriteRenderer.sprite = sprite;
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
            currFrame += inc;
            if(currFrame == frames - 1 || currFrame == 0) inc *= -1;
            tile.spriteRenderer.sprite = SpriteManager.GetTileSprite(name + "_" + currFrame.ToString());
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
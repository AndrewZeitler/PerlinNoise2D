using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteManager : MonoBehaviour
{
    public SpriteAtlas terrainSpriteSheet;

    public Sprite[] GetSprites(TileInfo tile){
        Sprite[] sprites;
        if(tile.isAnimation){
            sprites = new Sprite[tile.animationFrames];
            for(int i = 0; i < tile.animationFrames; ++i){
                sprites[i] = terrainSpriteSheet.GetSprite(tile.tileName + i);
            }
        } else {
            sprites = new Sprite[tile.amount];
            if(tile.hasAutoTiles) {
                if(tile.tileName.Contains("Center")){
                    for(int i = 0; i < tile.amount; ++i){
                        sprites[i] = terrainSpriteSheet.GetSprite(tile.tileName + i);
                    }
                } else {
                    sprites[0] = terrainSpriteSheet.GetSprite(tile.tileName);
                }
            } else {
                for(int i = 0; i < tile.amount; ++i){
                    sprites[i] = terrainSpriteSheet.GetSprite(tile.tileName + i);
                }
            }
        }
        return sprites;
    }
}

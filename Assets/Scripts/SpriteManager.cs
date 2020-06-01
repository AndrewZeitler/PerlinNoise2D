using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteManager : MonoBehaviour
{
    public SpriteAtlas terrainSpriteSheet;
    public SpriteAtlas itemSpriteSheet;

    private static SpriteAtlas terrainSprites;
    private static SpriteAtlas itemSprites;

    private void Start() {
        terrainSprites = terrainSpriteSheet;
        itemSprites = itemSpriteSheet;
    }

    public static Sprite[] GetSprites(TileInfo tile){
        Sprite[] sprites;
        if(tile.isAnimation){
            sprites = new Sprite[tile.animationFrames];
            for(int i = 0; i < tile.animationFrames; ++i){
                sprites[i] = terrainSprites.GetSprite(tile.tileName + i);
            }
        } else {
            sprites = new Sprite[tile.amount];
            if(tile.hasAutoTiles) {
                if(tile.tileName.Contains("Center")){
                    for(int i = 0; i < tile.amount; ++i){
                        sprites[i] = terrainSprites.GetSprite(tile.tileName + i);
                    }
                } else {
                    sprites[0] = terrainSprites.GetSprite(tile.tileName);
                }
            } else {
                for(int i = 0; i < tile.amount; ++i){
                    sprites[i] = terrainSprites.GetSprite(tile.tileName + i);
                }
            }
        }
        return sprites;
    }

    public static Sprite GetSprite(Item item){
        return itemSprites.GetSprite(item.name);
    }
}

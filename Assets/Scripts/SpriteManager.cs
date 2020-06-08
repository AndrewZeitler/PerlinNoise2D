using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Tiles;
using Items;

public class SpriteManager : MonoBehaviour
{
    public SpriteAtlas tileSpriteSheet;
    public SpriteAtlas itemSpriteSheet;

    private static SpriteAtlas tileSprites;
    private static SpriteAtlas itemSprites;

    private static Dictionary<string, Sprite> nameToTile;
    private static Dictionary<string, Sprite> nameToItem;

    private void Start() {
        tileSprites = tileSpriteSheet;
        itemSprites = itemSpriteSheet;

        nameToTile = new Dictionary<string, Sprite>();
        nameToItem = new Dictionary<string, Sprite>();

        Sprite[] sprites = new Sprite[tileSprites.spriteCount];
        tileSpriteSheet.GetSprites(sprites);
        foreach(Sprite sprite in sprites){
            nameToTile[sprite.name] = sprite;
        }

        sprites = new Sprite[itemSprites.spriteCount];
        itemSpriteSheet.GetSprites(sprites);
        foreach(Sprite sprite in sprites){
            nameToItem[sprite.name] = sprite;
        }
    }

    public static Sprite[] GetSprites(TileInfo tile){
        Sprite[] sprites;
        if(tile.isAnimation){
            sprites = new Sprite[tile.animationFrames];
            for(int i = 0; i < tile.animationFrames; ++i){
                sprites[i] = tileSprites.GetSprite(tile.tileName + i);
            }
        } else {
            sprites = new Sprite[tile.amount];
            if(tile.hasAutoTiles) {
                if(tile.tileName.Contains("Center")){
                    for(int i = 0; i < tile.amount; ++i){
                        sprites[i] = tileSprites.GetSprite(tile.tileName + i);
                    }
                } else {
                    sprites[0] = tileSprites.GetSprite(tile.tileName);
                }
            } else {
                for(int i = 0; i < tile.amount; ++i){
                    sprites[i] = tileSprites.GetSprite(tile.tileName + i);
                }
            }
        }
        return sprites;
    }

    // public static Sprite GetSprite(Item item){
    //     return itemSprites.GetSprite(item.itemData.Name);
    // }

    public static Sprite GetTileSprite(string name){
        return nameToTile[name + "(Clone)"];
    }

    public static Sprite GetItemSprite(string name){
        return nameToItem[name.ToLower() + "(Clone)"];
    }

    public static Sprite[] GetSprites(TileData tileData){
        return null;
    }
}

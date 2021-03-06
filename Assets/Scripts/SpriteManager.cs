﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Tiles;
using Utils;

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
            nameToTile[sprite.name.Replace("(Clone)", "")] = sprite;
        }

        sprites = new Sprite[itemSprites.spriteCount];
        itemSpriteSheet.GetSprites(sprites);
        foreach(Sprite sprite in sprites){
            nameToItem[sprite.name.Replace("(Clone)", "")] = sprite;
        }
    }

    public static Dictionary<string, Sprite> GetSprites(TileData tileData){
        Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
        AutoTileAnimated animation = tileData.GetModifierOfType<AutoTileAnimated>() as AutoTileAnimated;
        if(animation != null){
            for(int i = 0; i < animation.frames; ++i){
                foreach(TerrainBrush.TileType tileType in Enum.GetValues(typeof(TerrainBrush.TileType))) {
                    sprites[tileData.Name + tileType.ToString("") + i] = GetTileSprite(tileData.Name + tileType.ToString("") + i);
                }
            }
            return sprites;
        }
        AutoTile autoTile = tileData.GetModifierOfType<AutoTile>() as AutoTile;
        if(autoTile != null){
            foreach(TerrainBrush.TileType tileType in Enum.GetValues(typeof(TerrainBrush.TileType))) {
                sprites[tileData.Name + tileType.ToString("")] = GetTileSprite(tileData.Name + tileType.ToString(""));
            }
            return sprites;
        }
        TileRenderer renderer = tileData.GetModifierOfType<TileRenderer>() as TileRenderer;
        if(renderer != null){
            for(int i = 0; i < renderer.tileAmounts; ++i){
                sprites[tileData.Name + i] = GetTileSprite(tileData.Name + i);
            }
            return sprites;
        }
        return null;
    }

    public static Sprite GetTileSprite(string name){
        if(!nameToTile.ContainsKey(name)) {
            return null;
        }
        return nameToTile[name];
    }

    public static Sprite GetItemSprite(string name){
        if(!nameToItem.ContainsKey(name.ToLower())) {
            //Debug.Log(name);
            return null;
        }
        return nameToItem[name.ToLower()];
    }
}

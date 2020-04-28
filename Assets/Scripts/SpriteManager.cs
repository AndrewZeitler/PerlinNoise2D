using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteManager : MonoBehaviour
{
    public SpriteAtlas terrainSpriteSheet;

    [System.Serializable]
    public struct TileType {
        public string name;
        public int id;
        public int amount;
        public bool isAnimation;
        public int animationFrames;
        public float frameRate;
    }
    public TileType[] tileTypes;
    public float inverseVariationFactor;
    private Dictionary<string, TileType> typeToInfo;
    private Dictionary<string, Sprite[]> animations;

    string[] stringTypes = {"Center", "Top", "Right", "Bottom", "Left", "TopLeft", "TopRight", "BottomLeft", "BottomRight", 
                             "TopLeftL", "TopRightL", "BottomLeftL", "BottomRightL", "LeftDiagonal", "RightDiagonal"};

    public void Initialize() {
        typeToInfo = new Dictionary<string, TileType>();
        animations = new Dictionary<string, Sprite[]>();
        foreach(TileType tileType in tileTypes){
            typeToInfo[tileType.name] = tileType;
            if(tileType.isAnimation){
                foreach(string type in stringTypes){
                    Sprite[] frames = new Sprite[tileType.animationFrames];
                    for(int i = 0; i < tileType.animationFrames; ++i){
                        frames[i] = GetSprite(tileType.name + type + i);
                    }
                    animations[tileType.name + type] = frames;
                }
            }
        }
    }

    public Sprite GetSprite(string name){
        return terrainSpriteSheet.GetSprite(name);
    }

    public Sprite GetAutoTile(string name){
        string type = name.Split(new string[] {"Center"}, System.StringSplitOptions.None)[0];
        if(!type.Equals(name)){
            int rand = (int) Random.Range(0, (typeToInfo[type].amount - 1) * inverseVariationFactor);
            rand -= (int)((typeToInfo[type].amount - 1) * (inverseVariationFactor - 1));
            if(rand < 0) rand = 0;
            return GetSprite(name + rand);
        }
        return GetSprite(name);
    }

    public Sprite[] GetAnimationFrames(string name){
        return animations[name];
    }

    public float GetAnimationFrameRate(string name){
        return typeToInfo[name].frameRate;
    }
}

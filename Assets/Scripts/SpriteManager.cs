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
        public int amount;
        public bool isAnimation;
        public int animationFrames;
        public float frameRate;
    }
    public TileType[] tileTypes;
    public float inverseVariationFactor;
    private Dictionary<string, int> typeAmounts;
    private Dictionary<string, Sprite[]> animations;
    private Dictionary<string, float> frameRates;

    private string[] directions = {"Center", "Top", "Left", "Down", "Right", "Bottom", "TopLeft", "TopRight", "BottomLeft", 
                                   "BottomRight", "TopLeftL", "TopRightL", "BottomLeftL", "BottomRightL"};

    public void Initialize() {
        typeAmounts = new Dictionary<string, int>();
        animations = new Dictionary<string, Sprite[]>();
        frameRates = new Dictionary<string, float>();
        foreach(TileType tileType in tileTypes){
            typeAmounts[tileType.name] = tileType.amount;
            frameRates[tileType.name] = tileType.frameRate;
            if(tileType.isAnimation){
                foreach(string dir in directions){
                    Sprite[] frames = new Sprite[tileType.animationFrames];
                    for(int i = 0; i < tileType.animationFrames; ++i){
                        frames[i] = GetSprite(tileType.name + dir + i);
                    }
                    animations[tileType.name + dir] = frames;
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
            int rand = (int) Random.Range(0, (typeAmounts[type] - 1) * inverseVariationFactor);
            rand -= (int)((typeAmounts[type] - 1) * (inverseVariationFactor - 1));
            if(rand < 0) rand = 0;
            return GetSprite(name + rand);
        }
        return GetSprite(name);
    }

    public Sprite[] GetAnimationFrames(string name){
        return animations[name];
    }

    public float GetAnimationFrameRate(string name){
        return frameRates[name];
    }
}

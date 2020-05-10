using UnityEngine;

public class TileInfo {
    public string tileName;
    public int id;
    public int amount;
    public bool isAnimation;
    public int animationFrames;
    public float frameRate;
    public bool isTerrain;
    public float spawnChance;
    public float continuationBias;
    public int priority;
    public bool hasAutoTiles;
    public bool isResource;
    public float singleResourceChance;
    public float veinChance;
    public float veinContinuationBias;
    public string resourceSpawnTile;
    public Sprite[] sprites;

    public TileInfo(EditorTile et, int id){
        tileName = et.tileName;
        this.id = id;
        amount = et.amount;
        isAnimation = et.isAnimation;
        animationFrames = et.animationFrames;
        frameRate = et.frameRate;
        isTerrain = et.isTerrain;
        spawnChance = et.spawnChance;
        continuationBias = et.continuationBias;
        priority = et.priority;
        hasAutoTiles = et.generateAutoTiles;
        isResource = et.isResource;
        singleResourceChance = et.singleResourceChance;
        veinChance = et.veinChance;
        veinContinuationBias = et.veinContinuationBias;
        resourceSpawnTile = et.resourceSpawnTile;
    }

    public TileInfo(EditorTile et, string tileName, int id){
        this.tileName = tileName;
        this.id = id;
        if(tileName.Contains("Center")){
            amount = et.amount;
            isTerrain = et.isTerrain;
        } else {
            amount = 1;
            isTerrain = false;
        }
        isAnimation = et.isAnimation;
        animationFrames = et.animationFrames;
        frameRate = et.frameRate;
        spawnChance = et.spawnChance;
        continuationBias = et.continuationBias;
        priority = et.priority;
        hasAutoTiles = et.generateAutoTiles;
        isResource = et.isResource;
        singleResourceChance = et.singleResourceChance;
        veinChance = et.veinChance;
        veinContinuationBias = et.veinContinuationBias;
        resourceSpawnTile = et.resourceSpawnTile;
    }

    public static int ComparePriority(TileInfo thisInfo, TileInfo other){
        return other.priority.CompareTo(thisInfo.priority);
    }
}
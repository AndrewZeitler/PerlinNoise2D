
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
    }

    public TileInfo(EditorTile et, string tileName, int id){
        this.tileName = tileName;
        this.id = id;
        if(et.id == id){
            amount = et.amount;
        } else {
            amount = 1;
        }
        isAnimation = et.isAnimation;
        animationFrames = et.animationFrames;
        frameRate = et.frameRate;
        isTerrain = et.isTerrain;
        spawnChance = et.spawnChance;
        continuationBias = et.continuationBias;
        priority = et.priority;
    }
}
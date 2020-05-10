using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public struct EditorTile
{
    [System.Serializable]
    public struct AutoTile {
        public string tileName;
        public int id;
    }

    public string tileName;
    [System.Obsolete("ID is not assigned to.")]
    public int id;
    public int amount;
    public bool isWalkable;
    public bool isAnimation;
    public int animationFrames;
    public float frameRate;
    public bool isTerrain;
    public float spawnChance;
    public float continuationBias;
    public int priority;
    public bool generateAutoTiles;
    public AutoTile[] autoTiles;
    public bool isResource;
    public float singleResourceChance;
    public float veinChance;
    public float veinContinuationBias;
    public string resourceSpawnTile;
}

[System.Serializable]
public class TileManager : MonoBehaviour
{
    public SpriteManager spriteManager;
    public string defaultTileName;
    [HideInInspector]
    public TileInfo defaultTile;
    public GameObject tilePrefab;
    [HideInInspector]
    public List<EditorTile> editorTiles;
    [HideInInspector]
    public List<TileInfo> tiles;
    private Dictionary<string, TileInfo> nameToTile;

    private void Start() {
        int id = 0;
        tiles = new List<TileInfo>();
        nameToTile = new Dictionary<string, TileInfo>();

        foreach(EditorTile tile in editorTiles) {
            if(tile.generateAutoTiles){
                foreach(EditorTile.AutoTile auto in tile.autoTiles){
                    tiles.Add(new TileInfo(tile, auto.tileName, id));
                    ++id;
                }
            } else {
                tiles.Add(new TileInfo(tile, id));
                ++id;
            }
        }
        foreach(TileInfo tile in tiles){
            tile.sprites = spriteManager.GetSprites(tile);
            nameToTile.Add(tile.tileName, tile);
        }
        defaultTile = nameToTile[defaultTileName];
    }

    public TileInfo GetTile(int id){
        if(id < 0 || id >= tiles.Count) {return null;}
        return tiles[id];
    }

    public TileInfo GetTile(string name){
        if(!nameToTile.ContainsKey(name)) return null;
        return nameToTile[name];
    }

    public Sprite GetSprite(int id){
        return tiles[id].sprites[Random.Range(0, tiles[id].amount - 1)];
    }
}



#if UNITY_EDITOR
 [CustomEditor(typeof(TileManager))]
 public class TileInfo_Editor : Editor
 {
     List<bool> showTiles;
    string[] stringTypes = {"Center", "Top", "Right", "Bottom", "Left", "TopLeft", "TopRight", "BottomLeft", "BottomRight", 
                            "TopLeftL", "TopRightL", "BottomLeftL", "BottomRightL", "LeftDiagonal", "RightDiagonal"};
    TileManager manager;

    void OnEnable()
    {
        showTiles = new List<bool>();
        manager = (TileManager) target;
        if(manager.editorTiles == null) manager.editorTiles = new List<EditorTile>();
    }

     public override void OnInspectorGUI()
     {
        DrawDefaultInspector();
        
        EditorGUILayout.BeginVertical();
        EditorGUI.indentLevel += 1;

        while(manager.editorTiles.Count > showTiles.Count){
            showTiles.Add(false);
        }

        for(int i = 0; i < manager.editorTiles.Count; ++i){
            EditorTile script = manager.editorTiles[i];
            string name = script.tileName;
            if(name == null) name = "Tile #" + i;
            showTiles[i] = EditorGUILayout.Foldout(showTiles[i], name);

            if(!showTiles[i]) continue;

            script.tileName = EditorGUILayout.TextField("Tile Name", script.tileName);
            //script.id = EditorGUILayout.IntField("ID", script.id);
            script.amount = EditorGUILayout.IntField("Amount", script.amount);
            script.isWalkable = EditorGUILayout.Toggle("Is Walkable", script.isWalkable);
            script.isAnimation = EditorGUILayout.Toggle("Is Animation", script.isAnimation);
            if (script.isAnimation) {
                script.animationFrames = EditorGUILayout.IntField("Animation Frames", script.animationFrames);
                script.frameRate = EditorGUILayout.FloatField("Frame Rate", script.frameRate);
            }

            script.isTerrain = EditorGUILayout.Toggle("Is Terrain", script.isTerrain);
            if (script.isTerrain) {
                script.spawnChance = EditorGUILayout.FloatField("Spawn Chance", script.spawnChance);
                script.continuationBias = EditorGUILayout.FloatField("Continuation Bias", script.continuationBias);
                script.priority = EditorGUILayout.IntField("Priority", script.priority);
                script.generateAutoTiles = EditorGUILayout.Toggle("Generate Auto Tiles?", script.generateAutoTiles);

                if(script.generateAutoTiles){
                    if(script.autoTiles == null) script.autoTiles = new EditorTile.AutoTile[stringTypes.Length];
                    for(int j = 0; j < stringTypes.Length; ++j){
                        script.autoTiles[j].tileName = script.tileName + stringTypes[j];
                    }
                }
            }
            script.isResource = EditorGUILayout.Toggle("Is Resource", script.isResource);
            if(script.isResource){
                script.singleResourceChance = EditorGUILayout.FloatField("Single Resource Chance", script.singleResourceChance);
                script.veinChance = EditorGUILayout.FloatField("Vein Chance", script.veinChance);
                script.veinContinuationBias = EditorGUILayout.FloatField("Vein Continuation Bias", script.veinContinuationBias);
                script.resourceSpawnTile = EditorGUILayout.TextField("Resource Spawn Tile", script.resourceSpawnTile);
            }
            manager.editorTiles[i] = script;
        }
        EditorGUI.indentLevel -= 1;
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add tile")) {
            manager.editorTiles.Add(new EditorTile());
            showTiles.Add(false);
        }
        if (GUILayout.Button("Delete tile")) {
            manager.editorTiles.RemoveAt(manager.editorTiles.Count - 1);
            showTiles.RemoveAt(showTiles.Count - 1);
        }
        EditorGUILayout.EndHorizontal();

     }
 }
 #endif

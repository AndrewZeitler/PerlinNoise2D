using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct TileInfo {
        public string name;
        public int id;
        public Vector2 randomRange;
        public float continuationBias;
        public bool isAnimation;
    }
    public string defaultTile = "grass";
    public int size = 16;
    public float zoom = 16;
    public GameObject tile;
    public TileInfo[] tilesInfo;
    int seed;

    Dictionary<string, int> tileTypes;
    Dictionary<int, string> typeToTile;
    Dictionary<int, float> idToContinuation;
    Dictionary<Vector2, string> tileRanges;
    Dictionary<int, Vector2> idToRange;
    Dictionary<int, bool> tileAnimationState;

    void Start()
    {
        seed = System.Environment.TickCount;
        tileTypes = new Dictionary<string, int>();
        typeToTile = new Dictionary<int, string>();
        tileRanges = new Dictionary<Vector2, string>();
        idToContinuation = new Dictionary<int, float>();
        idToRange = new Dictionary<int, Vector2>();
        tileAnimationState = new Dictionary<int, bool>();
        

        foreach(TileInfo info in tilesInfo){
            tileTypes[info.name] = info.id;
            typeToTile[info.id] = info.name;
            tileRanges[info.randomRange] = info.name;
            idToContinuation[info.id] = info.continuationBias;
            idToRange[info.id] = info.randomRange;
            tileAnimationState[info.id] = info.isAnimation;
        }
    }

    // Gets the tile type from the random value generated
    string GetTypeFromRand(Chunk chunk, int x, int y, double rand){
        int otherType = -1;
        int defaultType = tileTypes[defaultTile];
        for(int xd = -1; xd < 2 && otherType == -1; ++xd){
            for(int yd = -1; yd < 2 && otherType == -1; ++yd){
                if(x + xd < 0 || x + xd >= Chunk.chunkSize || y + yd < 0 || y + yd >= Chunk.chunkSize) continue;
                if(chunk.grid[x + xd, y + yd] == null) continue;
                if(chunk.grid[x + xd, y + yd].id != defaultType) otherType = chunk.grid[x + xd, y + yd].id;
            }
        }

        if(otherType != -1){
            if(rand >= idToRange[otherType].x - idToContinuation[otherType] && 
               rand <= idToRange[otherType].y + idToContinuation[otherType]){
                return typeToTile[otherType];
            }
            return defaultTile;
        }

        foreach (var item in tileRanges){
            if(rand >= item.Key.x && rand <= item.Key.y){
                return item.Value;
            }
        }
        return "";
    }
    public void GenerateChunk(Chunk chunk){;
        for(int x = 0; x < Chunk.chunkSize; x++){
            for(int y = 0; y < Chunk.chunkSize; y++){
                double result = Mathf.PerlinNoise((x + Chunk.chunkSize * chunk.x + System.Int16.MaxValue / 2) / zoom, 
                                                  (y + Chunk.chunkSize * chunk.y + System.Int16.MaxValue / 2) / zoom);
                result = Mathf.Clamp((float) result, 0f, 1.0f);
                chunk.grid[x, y] = new Tile(tileTypes[GetTypeFromRand(chunk, x, y, result)]);
            }
        }
    }

    public void GenerateChunk2(Chunk chunk){
        int defaultId = tileTypes[defaultTile];
        for(int x = 0; x < Chunk.chunkSize; ++x){
            for(int y = 0; y < Chunk.chunkSize; ++y){
                chunk.grid[x,y] = new Tile(defaultId);
            }
        }
        foreach(TileInfo tileInfo in tilesInfo){
            if(tileInfo.name == defaultTile) continue;
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){

                }
            }
        }
    }

    public IEnumerator GenerateChunkConcurrently(Chunk chunk){;
        for(int x = 0; x < Chunk.chunkSize; x++){
            for(int y = 0; y < Chunk.chunkSize; y++){
                double result = Mathf.PerlinNoise((x + Chunk.chunkSize * chunk.x + System.Int16.MaxValue / 2) / zoom, 
                                                  (y + Chunk.chunkSize * chunk.y + System.Int16.MaxValue / 2) / zoom);
                result = Mathf.Clamp((float) result, 0f, 1.0f);
                chunk.grid[x, y] = new Tile(tileTypes[GetTypeFromRand(chunk, x, y, result)]);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void MakeChunkSprites(Chunk[,] chunks, int xChunk, int yChunk){
        SpriteManager spriteManager = GetComponent<SpriteManager>();
        spriteManager.Initialize();
        for(int x = 0; x < Chunk.chunkSize; x++){
            for(int y = 0; y < Chunk.chunkSize; y++){
                GameObject sprite = Instantiate(tile, new Vector2(x + chunks[xChunk, yChunk].x * Chunk.chunkSize, 
                                                (y + chunks[xChunk, yChunk].y * Chunk.chunkSize)), Quaternion.identity);
                //sprite.transform.SetParent(transform);
                sprite.transform.localScale = new Vector2(1 / 0.16f, 1 / 0.16f);
                if(tileAnimationState[chunks[xChunk, yChunk].grid[x,y].id]){
                    string name = typeToTile[chunks[xChunk, yChunk].grid[x,y].id];
                    sprite.GetComponent<TileAnimator>().SetFrames(spriteManager.GetAnimationFrames(GetChunkSprite(chunks, xChunk, yChunk, x, y)),
                                                                    spriteManager.GetAnimationFrameRate(name));
                } else {
                    if(chunks[xChunk, yChunk].grid[x,y].id == 0){
                        sprite.GetComponent<SpriteRenderer>().sprite = spriteManager.GetAutoTile("grassCenter");
                    } else {
                        sprite.GetComponent<SpriteRenderer>().sprite = 
                            spriteManager.GetAutoTile(GetChunkSprite(chunks, xChunk, yChunk, x, y));
                    }
                }
                chunks[xChunk,yChunk].grid[x,y].tile = sprite;
            }
        }
    }

    // Gets the local direction of the tile in relation to the surrounding tiles
    string GetChunkSprite(Chunk[,] chunks, int xChunk, int yChunk, int x, int y){
        int type1 = chunks[xChunk, yChunk].grid[x, y].id;
        int type2 = 0;
        int type1Count = 0;
        int[,] filter = new int[3, 3];
        for(int dx = -1; dx < 2; dx++){
            for(int dy = -1; dy < 2; dy++){
                if(x + dx < 0 || x + dx >= Chunk.chunkSize || y + dy < 0 || y + dy >= Chunk.chunkSize) {
                    int cx = xChunk;
                    int cy = yChunk;
                    int nx = x + dx;
                    int ny = y + dy;
                    if(x + dx < 0 || x + dx >= Chunk.chunkSize) {
                        nx = (dx < 0 ? Chunk.chunkSize - 1 : 0);
                        cx = xChunk + dx;
                    }
                    if(y + dy < 0 || y + dy >= Chunk.chunkSize) {
                        ny = (dy < 0 ? Chunk.chunkSize - 1 : 0);
                        cy = yChunk + dy;
                    }
                    filter[dx + 1, dy + 1] = chunks[cx,cy].grid[nx, ny].id;
                    if(filter[dx + 1, dy + 1] == type1) ++type1Count;
                    else type2 = filter[dx + 1, dy + 1];
                    continue;
                }
                if(chunks[xChunk,yChunk].grid[x + dx,y + dy].id == type1) ++type1Count;
                else type2 = chunks[xChunk,yChunk].grid[x + dx,y + dy].id;
                filter[dx + 1, dy + 1] = chunks[xChunk,yChunk].grid[x + dx,y + dy].id;
            }
        }
        // These are all cases where the center tile is jutting out
        if(type1Count == 1 || type1Count == 2 || type1Count == 3) return typeToTile[type2] + "Center";
        if(type1Count == 4) {
            // Check if the tiles form a square in a corner
            if((filter[0,1] == type1 && (filter[1, 2] == type1 || filter[1, 0] == type1)) ||
                (filter[2, 1] == type1 && (filter[1, 2] == type1 || filter[1, 0] == type1))){
                    string result = typeToTile[type1];
                    if(filter[0,1] == type1 && filter[0,0] == type1 && filter[1,0] == type1) result += "BottomLeft";
                    else if(filter[0,1] == type1 && filter[0,2] == type1 && filter[1,2] == type1) result += "TopLeft";
                    else if(filter[2,1] == type1 && filter[2,0] == type1 && filter[1,0] == type1) result += "BottomRight";
                    else if(filter[2,1] == type1 && filter[2,2] == type1 && filter[1,2] == type1) result += "TopRight";
                    return result;
            }
            // All other cases have middle block on its own and shoulld be turned to grass
            return typeToTile[type2] + "Center";
        }
        if(type1Count == 5 || type1Count == 6 || type1Count == 7){
            string result = typeToTile[type1];
            if(filter[0,1] == type1 && filter[0,0] == type1 && filter[1,0] == type1) result += "BottomLeft";
            if(filter[0,1] == type1 && filter[0,2] == type1 && filter[1,2] == type1) result += "TopLeft";
            if(filter[2,1] == type1 && filter[2,0] == type1 && filter[1,0] == type1) result += "BottomRight";
            if(filter[2,1] == type1 && filter[2,2] == type1 && filter[1,2] == type1) result += "TopRight";
            if(result == typeToTile[type1]) return typeToTile[type2] + "Center";
            if(result.Split(new string[] {"Top"}, System.StringSplitOptions.None).Length > 2) return typeToTile[type1] + "Top";
            if(result.Split(new string[] {"Left"}, System.StringSplitOptions.None).Length > 2) return typeToTile[type1] + "Left";
            if(result.Split(new string[] {"Right"}, System.StringSplitOptions.None).Length > 2) return typeToTile[type1] + "Right";
            if(result.Split(new string[] {"Bottom"}, System.StringSplitOptions.None).Length > 2) return typeToTile[type1] + "Bottom";
            if(result.Contains("Top") && result.Contains("Bottom")){
                if(result.IndexOf("Top") < result.IndexOf("Bottom")){
                    return typeToTile[type1] + "RightDiagonal";
                } else {
                    return typeToTile[type1] + "LeftDiagonal";
                }
            }
            return result;
        }
        if(type1Count == 8){
            string result = typeToTile[type1];
            if(filter[0,0] == type2) result += "TopRightL";
            else if(filter[1,0] == type2) result += "Top";
            else if(filter[2,0] == type2) result += "TopLeftL";
            else if(filter[0,1] == type2) result += "Right";
            else if(filter[2,1] == type2) result += "Left";
            else if(filter[0,2] == type2) result += "BottomRightL";
            else if(filter[1,2] == type2) result += "Bottom";
            else result += "BottomLeftL";
            return result;
        }
        return typeToTile[type1] + "Center";
    }

    
    // Generate base terrain values
    /**void GenerateTerrain(){
        world = new int[size, size];
        for(int x = 0; x < size; x++){
            for(int y = 0; y < size; y++){
                double result = Mathf.PerlinNoise(x / zoom, y / zoom);
                result = Mathf.Clamp((float) result, 0f, 1.0f);
                world[x, y] = tileTypes[GetTypeFromRand(result)];
            }
        }
    }

    void RemoveBordering(){
        for(int x = 0; x < size; ++x){
            for(int y = 0; y < size; ++y){
                for(int dx = -1; dx < 2; ++dx){
                    for(int dy = -1; dy < 2; ++dy){
                        if(x + dx < 0 || x + dx == size || y + dy < 0 || y + dy == size) continue;
                        if(world[x + dx, y + dy] != 0 && world[x + dx, y + dy] != world[x,y]) {
                            world[x, y] = 0;
                        }
                    }
                }
            }
        }
    }

    void MakeSprites(){
        SpriteManager spriteManager = GetComponent<SpriteManager>();
        spriteManager.Initialize();
        terrain = new GameObject[size, size];
        for(int x = 0; x < size; x++){
            for(int y = 0; y < size; y++){
                terrain[x,y] = Instantiate(tile, new Vector2(x, -y), Quaternion.identity);
                terrain[x,y].transform.SetParent(transform);
                terrain[x,y].transform.localScale = new Vector2(1 / 0.16f, 1 / 0.16f);
                if(world[x,y] == 0){
                    terrain[x,y].GetComponent<SpriteRenderer>().sprite = spriteManager.GetAutoTile("grassCenter");
                } else {
                    terrain[x,y].GetComponent<SpriteRenderer>().sprite = spriteManager.GetAutoTile(GetSprite(x, y));
                }
            }
        }
    }

    // Gets the local direction of the tile in relation to the surrounding tiles
    string GetSprite(int x, int y){
        int type1 = world[x, y];
        int type2 = 0;
        int type1Count = 0;
        int[,] filter = new int[3, 3];
        for(int dx = -1; dx < 2; dx++){
            for(int dy = -1; dy < 2; dy++){
                if(x + dx < 0 || x + dx >= size || y + dy < 0 || y + dy >= size) {
                    filter[dx + 1, dy + 1] = type1;
                    ++type1Count;
                    continue;
                }
                if(world[x + dx,y + dy] == type1) ++type1Count;
                else type2 = world[x + dx,y + dy];
                filter[dx + 1, dy + 1] = world[x + dx, y + dy];
            }
        }
        // These are all cases where the center tile is jutting out
        if(type1Count == 1 || type1Count == 2 || type1Count == 3) return typeToTile[type2] + "Center";
        if(type1Count == 4) {
            // Check if the tiles form a square in a corner
            if((filter[0,1] == type1 && (filter[1, 2] == type1 || filter[1, 0] == type1)) ||
                (filter[2, 1] == type1 && (filter[1, 2] == type1 || filter[1, 0] == type1))){
                    string result = typeToTile[type1];
                    if(filter[0,1] == type1 && filter[0,0] == type1 && filter[1,0] == type1) result += "TopLeft";
                    else if(filter[0,1] == type1 && filter[0,2] == type1 && filter[1,2] == type1) result += "BottomLeft";
                    else if(filter[2,1] == type1 && filter[2,0] == type1 && filter[1,0] == type1) result += "TopRight";
                    else if(filter[2,1] == type1 && filter[2,2] == type1 && filter[1,2] == type1) result += "BottomRight";
                    return result;
            }
            // All other cases have middle block on its own and shoulld be turned to grass
            return typeToTile[type2] + "Center";
        }
        if(type1Count == 5 || type1Count == 6 || type1Count == 7){
            string result = typeToTile[type1];
            if(filter[0,1] == type1 && filter[0,0] == type1 && filter[1,0] == type1) result += "TopLeft";
            if(filter[0,1] == type1 && filter[0,2] == type1 && filter[1,2] == type1) result += "BottomLeft";
            if(filter[2,1] == type1 && filter[2,0] == type1 && filter[1,0] == type1) result += "TopRight";
            if(filter[2,1] == type1 && filter[2,2] == type1 && filter[1,2] == type1) result += "BottomRight";
            if(result == typeToTile[type1]) return typeToTile[type2] + "Center";
            if(result.Split(new string[] {"Top"}, System.StringSplitOptions.None).Length > 2) return typeToTile[type1] + "Top";
            if(result.Split(new string[] {"Left"}, System.StringSplitOptions.None).Length > 2) return typeToTile[type1] + "Left";
            if(result.Split(new string[] {"Right"}, System.StringSplitOptions.None).Length > 2) return typeToTile[type1] + "Right";
            if(result.Split(new string[] {"Bottom"}, System.StringSplitOptions.None).Length > 2) return typeToTile[type1] + "Bottom";
            return result;
        }
        if(type1Count == 8){
            string result = typeToTile[type1];
            if(filter[0,0] == type2) result += "BottomRightL";
            else if(filter[1,0] == type2) result += "Bottom";
            else if(filter[2,0] == type2) result += "BottomLeftL";
            else if(filter[0,1] == type2) result += "Right";
            else if(filter[2,1] == type2) result += "Left";
            else if(filter[0,2] == type2) result += "TopRightL";
            else if(filter[1,2] == type2) result += "Top";
            else result += "TopLeftL";
            return result;
        }
        return typeToTile[type1] + "Center";
    } **/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct TileInfo {
        public string name;
        public int id;
        public float spawnChance;
        public float continuationBias;
        public bool isAnimation;
        public int priority;
    }
    public string defaultTile = "grass";
    public float zoom = 16;
    public GameObject tile;
    public TileInfo[] tilesInfo;
    int seed;

    Dictionary<string, int> tileTypes;
    Dictionary<int, TileInfo> typeToInfo;

    void Start()
    {
        seed = System.Environment.TickCount;
        Random.InitState(seed);
        tileTypes = new Dictionary<string, int>();
        typeToInfo = new Dictionary<int, TileInfo>();
        

        foreach(TileInfo info in tilesInfo){
            tileTypes[info.name] = info.id;
            typeToInfo[info.id] = info;
        }
    }

    // Gets the tile type from the random value generated
    // string GetTypeFromRand(Chunk chunk, int x, int y, double rand){
    //     int otherType = -1;
    //     int defaultType = tileTypes[defaultTile];
    //     for(int xd = -1; xd < 2 && otherType == -1; ++xd){
    //         for(int yd = -1; yd < 2 && otherType == -1; ++yd){
    //             if(x + xd < 0 || x + xd >= Chunk.chunkSize || y + yd < 0 || y + yd >= Chunk.chunkSize) continue;
    //             if(chunk.grid[x + xd, y + yd] == null) continue;
    //             if(chunk.grid[x + xd, y + yd].id != defaultType) otherType = chunk.grid[x + xd, y + yd].id;
    //         }
    //     }

    //     if(otherType != -1){
    //         if(rand >= idToRange[otherType].x - idToContinuation[otherType] && 
    //            rand <= idToRange[otherType].y + idToContinuation[otherType]){
    //             return typeToTile[otherType];
    //         }
    //         return defaultTile;
    //     }

    //     foreach (var item in tileRanges){
    //         if(rand >= item.Key.x && rand <= item.Key.y){
    //             return item.Value;
    //         }
    //     }
    //     return "";
    // }
    // public void GenerateChunk(Chunk chunk){;
    //     for(int x = 0; x < Chunk.chunkSize; x++){
    //         for(int y = 0; y < Chunk.chunkSize; y++){
    //             double result = Mathf.PerlinNoise((x + Chunk.chunkSize * chunk.x + System.Int16.MaxValue / 2) / zoom, 
    //                                               (y + Chunk.chunkSize * chunk.y + System.Int16.MaxValue / 2) / zoom);
    //             result = Mathf.Clamp((float) result, 0f, 1.0f);
    //             chunk.grid[x, y] = new Tile(tileTypes[GetTypeFromRand(chunk, x, y, result)]);
    //         }
    //     }
    // }
    // public IEnumerator GenerateChunkConcurrently(Chunk chunk){;
    //     for(int x = 0; x < Chunk.chunkSize; x++){
    //         for(int y = 0; y < Chunk.chunkSize; y++){
    //             double result = Mathf.PerlinNoise((x + Chunk.chunkSize * chunk.x + System.Int16.MaxValue / 2) / zoom, 
    //                                               (y + Chunk.chunkSize * chunk.y + System.Int16.MaxValue / 2) / zoom);
    //             result = Mathf.Clamp((float) result, 0f, 1.0f);
    //             chunk.grid[x, y] = new Tile(tileTypes[GetTypeFromRand(chunk, x, y, result)]);
    //         }
    //         yield return new WaitForEndOfFrame();
    //     }
    // }

    bool isInChunk(int x, int y){
        return !(x < 0 || x >= Chunk.chunkSize || y < 0 || y >= Chunk.chunkSize);
    }

    bool isContinuation(Chunk chunk, int x, int y, int id){
        for(int xd = -1; xd < 2; ++xd){
            for(int yd = -1; yd < 2; ++yd){
                if(!isInChunk(x + xd, y + yd)) continue;
                if(chunk.grid[x + xd, y + yd].id == id) return true;
            }
        }
        return false;
    }

    public void GenerateChunk(Chunk chunk){
        // Set all values to default in chunk
        int defaultId = tileTypes[defaultTile];
        for(int x = 0; x < Chunk.chunkSize; ++x){
            for(int y = 0; y < Chunk.chunkSize; ++y){
                chunk.grid[x,y] = new Tile(defaultId);
            }
        }
        // Then loop through each tile type except the default one
        foreach(TileInfo tileInfo in tilesInfo){
            if(tileInfo.name == defaultTile) continue;
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    // Check to see if the priority is already above current tile
                    if(typeToInfo[chunk.grid[x,y].id].priority > tileInfo.priority) continue;
                    // Check if we are continuing spawned tile
                    float seed = Random.value;
                    if(isContinuation(chunk, x, y, tileInfo.id)){
                        double rand = Mathf.PerlinNoise((x + chunk.x * Chunk.chunkSize + seed * tileInfo.id) / zoom, 
                                                        (y + chunk.y * Chunk.chunkSize + seed * tileInfo.id) / zoom);
                        if(rand <= tileInfo.spawnChance + tileInfo.continuationBias){
                            chunk.grid[x,y].id = tileInfo.id;
                        } else {
                            chunk.grid[x,y].id = tileInfo.id;
                        }
                    } else {
                        // Otherwise check if perlin noise value is less than the spawn chance of the tile
                        double rand = Mathf.PerlinNoise((x + chunk.x * Chunk.chunkSize + seed * tileInfo.id) / zoom, 
                                                        (y + chunk.y * Chunk.chunkSize + seed * tileInfo.id) / zoom);
                        if(rand <= tileInfo.spawnChance){
                            chunk.grid[x, y].id = tileInfo.id;
                        }
                    }
                }
            }
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
                if(typeToInfo[chunks[xChunk, yChunk].grid[x,y].id].isAnimation){
                    string name = typeToInfo[chunks[xChunk, yChunk].grid[x,y].id].name;
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

    string[,] GetAutoTileTypes(Chunk[,] chunks, int xChunk, int yChunk){
        string[,] autoTileNames = new string[Chunk.chunkSize, Chunk.chunkSize];
        for(int x = 0; x < Chunk.chunkSize; ++x){
            for(int y = 0; y < Chunk.chunkSize; ++y){
                if(!typeToInfo[chunks[xChunk, yChunk].grid[x,y].id].isAnimation){
                    if(chunks[xChunk, yChunk].grid[x,y].id != tileTypes[defaultTile]) {
                        //autoTileNames[x,y] = G
                    }
                }
            }
        }
        return null;
    }
    

    // Gets the local direction of the tile in relation to the surrounding tiles
    string GetChunkSprite(Chunk[,] chunks, int xChunk, int yChunk, int x, int y){
        int type1 = chunks[xChunk, yChunk].grid[x, y].id;
        int type2 = tileTypes[defaultTile];
        int type1Count = 0;
        int[,] filter = new int[3, 3];
        for(int dx = -1; dx < 2; dx++){
            for(int dy = -1; dy < 2; dy++){
                if(!isInChunk(x + dx, y + dy)) {
                    int cx = xChunk;
                    int cy = yChunk;
                    int nx = x + dx;
                    int ny = y + dy;
                    if(!isInChunk(x + dx, y)) {
                        nx = (dx < 0 ? Chunk.chunkSize - 1 : 0);
                        cx = xChunk + dx;
                    }
                    if(!isInChunk(x, y + dy)) {
                        ny = (dy < 0 ? Chunk.chunkSize - 1 : 0);
                        cy = yChunk + dy;
                    }
                    filter[dx + 1, dy + 1] = chunks[cx,cy].grid[nx, ny].id;
                    if(filter[dx + 1, dy + 1] == type1) ++type1Count;
                    continue;
                }
                if(chunks[xChunk,yChunk].grid[x + dx,y + dy].id == type1) ++type1Count;
                filter[dx + 1, dy + 1] = chunks[xChunk,yChunk].grid[x + dx,y + dy].id;
            }
        }
        // These are all cases where the center tile is jutting out
        if(type1Count == 1 || type1Count == 2 || type1Count == 3) return typeToInfo[type2].name + "Center";
        if(type1Count == 4) {
            // Check if the tiles form a square in a corner
            if((filter[0,1] == type1 && (filter[1, 2] == type1 || filter[1, 0] == type1)) ||
                (filter[2, 1] == type1 && (filter[1, 2] == type1 || filter[1, 0] == type1))){
                    string result = typeToInfo[type1].name;
                    if(filter[0,1] == type1 && filter[0,0] == type1 && filter[1,0] == type1) result += "BottomLeft";
                    else if(filter[0,1] == type1 && filter[0,2] == type1 && filter[1,2] == type1) result += "TopLeft";
                    else if(filter[2,1] == type1 && filter[2,0] == type1 && filter[1,0] == type1) result += "BottomRight";
                    else if(filter[2,1] == type1 && filter[2,2] == type1 && filter[1,2] == type1) result += "TopRight";
                    return result;
            }
            // All other cases have middle block on its own and shoulld be turned to grass
            return typeToInfo[type2].name + "Center";
        }
        if(type1Count == 5 || type1Count == 6 || type1Count == 7){
            string result = typeToInfo[type1].name;
            if(filter[0,1] == type1 && filter[0,0] == type1 && filter[1,0] == type1) result += "BottomLeft";
            if(filter[0,1] == type1 && filter[0,2] == type1 && filter[1,2] == type1) result += "TopLeft";
            if(filter[2,1] == type1 && filter[2,0] == type1 && filter[1,0] == type1) result += "BottomRight";
            if(filter[2,1] == type1 && filter[2,2] == type1 && filter[1,2] == type1) result += "TopRight";
            if(result == typeToInfo[type1].name) return typeToInfo[type2].name + "Center";
            if(result.Split(new string[] {"Top"}, System.StringSplitOptions.None).Length > 2) return typeToInfo[type1].name + "Top";
            if(result.Split(new string[] {"Left"}, System.StringSplitOptions.None).Length > 2) return typeToInfo[type1].name + "Left";
            if(result.Split(new string[] {"Right"}, System.StringSplitOptions.None).Length > 2) return typeToInfo[type1].name + "Right";
            if(result.Split(new string[] {"Bottom"}, System.StringSplitOptions.None).Length > 2) return typeToInfo[type1].name + "Bottom";
            if(result.Contains("Top") && result.Contains("Bottom")){
                if(result.IndexOf("Top") < result.IndexOf("Bottom")){
                    return typeToInfo[type1].name + "RightDiagonal";
                } else {
                    return typeToInfo[type1].name + "LeftDiagonal";
                }
            }
            return result;
        }
        if(type1Count == 8){
            string result = typeToInfo[type1].name;
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
        return typeToInfo[type1].name + "Center";
    }
}

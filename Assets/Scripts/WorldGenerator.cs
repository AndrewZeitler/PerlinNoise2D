using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public TileManager tileManager;
    public double zoom = 16;
    int seed;
    List<TileInfo> terrainTiles;
    public int smoothPadding;
    public int renderedPadding;
    public int smoothBrushSize;
    PerlinNoiseGenerator generator;

    void Start()
    {
        generator = new PerlinNoiseGenerator();
        seed = System.Environment.TickCount / 10;
        seed = 100378114;
        Debug.Log("Seed: " + seed);

        terrainTiles = new List<TileInfo>();
        foreach(TileInfo tile in tileManager.tiles){
            if(tile.isTerrain){
                terrainTiles.Add(tile);
            }
        }
        terrainTiles.Sort(TileInfo.ComparePriority);
    }

    bool isContinuation(Chunk[,] chunks, int xChunk, int yChunk, int x, int y, int id){
        for(int xd = -1; xd < 2; ++xd){
            for(int yd = -1; yd < 2; ++yd){
                if(GetId(chunks, x + xd + xChunk * Chunk.chunkSize, y + yd + yChunk * Chunk.chunkSize) == id) return true;
            }
        }
        return false;
    }

    bool NeighbourIsAnimation(Chunk[,] chunks, int xChunk, int yChunk, int x, int y, int id){
        for(int xd = -1; xd < 2; ++xd){
            for(int yd = -1; yd < 2; ++yd){
                int tileId = GetId(chunks, x + xd + xChunk * Chunk.chunkSize, y + yd + yChunk * Chunk.chunkSize);
                if(tileId == id || tileId == -1) continue;
                if(tileManager.GetTile(tileId).isAnimation) return true;
            }
        }
        return false;
    }

    public int[,] CreateChunksArray(Chunk[,] chunks){
        int[,] ids = new int[chunks.GetLength(0) * Chunk.chunkSize, chunks.GetLength(1) * Chunk.chunkSize];
        for(int x = 0; x < chunks.GetLength(0); ++x){
            for(int y = 0; y < chunks.GetLength(1); ++y){
                for(int xc = 0; xc < Chunk.chunkSize; ++xc){
                    for(int yc = 0; yc < Chunk.chunkSize; ++yc){
                        ids[x * Chunk.chunkSize + xc, y * Chunk.chunkSize + yc] = chunks[x,y].grid[xc,yc].id;
                    }
                }
            }
        }
        return ids;
    }

    public void SaveChunksArray(Chunk[,] chunks, int[,] ids){
        for(int x = 0; x < chunks.GetLength(0); ++x){
            for(int y = 0; y < chunks.GetLength(1); ++y){
                if(chunks[x,y].chunkState != ChunkState.Smoothed) continue;
                for(int xc = 0; xc < Chunk.chunkSize; ++xc){
                    for(int yc = 0; yc < Chunk.chunkSize; ++yc){
                        chunks[x,y].grid[xc,yc].id = ids[x * Chunk.chunkSize + xc, y * Chunk.chunkSize + yc];
                    }
                }
                chunks[x,y].chunkState = ChunkState.Saved;
            }
        }
    }

    public int GetId(Chunk[,] chunks, int x, int y){
        int xChunk = x / Chunk.chunkSize;
        int yChunk = y / Chunk.chunkSize;
        int xc = x % Chunk.chunkSize;
        int yc = y % Chunk.chunkSize;
        if(xc < 0 || yc < 0) return -1;
        if(xChunk >= chunks.GetLength(0) || yChunk >= chunks.GetLength(1)) return -1;
        return chunks[xChunk, yChunk].grid[xc, yc].id;
    }

    public void LoadChunks(Chunk[,] chunks){
        for(int x = 0; x < chunks.GetLength(0); ++x){
            for(int y = 0; y < chunks.GetLength(1); ++y){
                if(chunks[x,y].chunkState == ChunkState.NotGenerated){
                    for(int xc = 0; xc < Chunk.chunkSize; ++xc){
                        for(int yc = 0; yc < Chunk.chunkSize; ++yc){
                            chunks[x,y].grid[xc,yc] = new Tile(tileManager.defaultTile.id);
                        }
                    }
                }
            }
        }
        for(int x = 0; x < chunks.GetLength(0); ++x){
            for(int y = 0; y < chunks.GetLength(1); ++y){
                if(chunks[x,y].chunkState == ChunkState.NotGenerated){
                    GenerateChunk(chunks, x, y);
                    chunks[x,y].chunkState = ChunkState.Generated;
                }
            }
        }
        for(int x = smoothPadding; x < chunks.GetLength(0) - smoothPadding; ++x){
            for(int y = smoothPadding; y < chunks.GetLength(1) - smoothPadding; ++y){
                if(chunks[x,y].chunkState == ChunkState.Generated){
                    //WidenChunk(chunks, x, y);
                }
            }
        }
        int[,] ids = CreateChunksArray(chunks);
        int[,] savedIds = ids.Clone() as int[,];
        for(int x = smoothPadding; x < chunks.GetLength(0) - smoothPadding; ++x){
            for(int y = smoothPadding; y < chunks.GetLength(1) - smoothPadding; ++y){
                if(chunks[x,y].chunkState == ChunkState.Generated){
                    SmoothChunk(ids, savedIds, x, y);
                    chunks[x,y].chunkState = ChunkState.Smoothed;
                }
            }
        }
        SaveChunksArray(chunks, ids);
        for(int x = renderedPadding; x < chunks.GetLength(0) - renderedPadding; ++x){
            for(int y = renderedPadding; y < chunks.GetLength(1) - renderedPadding; ++y){
                if(chunks[x,y].chunkState == ChunkState.Saved){
                    MakeChunkSprites(chunks[x,y]);
                    chunks[x,y].chunkState = ChunkState.Rendered;
                }
            }
        }
    }

    double PerlinNoise(double x, double y, int id){
        return generator.perlin((x + seed * id) / (zoom), (y + seed * id) / (zoom), seed / 10000000);
    }

    void GenerateChunk(Chunk[,] chunks, int xChunk, int yChunk){
        Chunk chunk = chunks[xChunk, yChunk];
        // Then loop through each tile type except the default one
        int i = 1;
        foreach(TileInfo tileInfo in terrainTiles){
            if(tileInfo.id == tileManager.defaultTile.id) continue;
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    // Check to see if the priority is already above current tile
                    if(chunk.grid[x,y].id != tileManager.defaultTile.id) continue;
                    //if(NeighbourIsAnimation(chunks, xChunk, yChunk, x, y, tileInfo.id)) continue;
                    double rand = PerlinNoise(x + chunk.x * Chunk.chunkSize, y + chunk.y * Chunk.chunkSize, i);
                    // if(isContinuation(chunks, xChunk, yChunk, x, y, tileInfo.id)){
                    //     if(rand <= tileInfo.spawnChance + tileInfo.continuationBias){
                    //         chunk.grid[x,y].id = tileInfo.id;
                    //     } else {
                    //         chunk.grid[x,y].id = tileManager.defaultTile.id;
                    //     }
                    // } else {
                    //     // Otherwise check if perlin noise value is less than the spawn chance of the tile
                        if(rand <= tileInfo.spawnChance){
                            chunk.grid[x, y].id = tileInfo.id;
                            chunk.rawIds[x, y] = tileInfo.id;
                        }
                    // }
                }
            }
            ++i;
        }
    }

    void WidenChunk(Chunk[,] chunks, int xChunk, int yChunk){
        foreach(TileInfo tile in terrainTiles){
            if(tile.id == tileManager.defaultTile.id) continue;
            int[,] ids = new int[Chunk.chunkSize * 3, Chunk.chunkSize * 3];
            TerrainBrush terrainBrush = new TerrainBrush(tile.brushSize);
            for(int xd = -1; xd < 2; ++xd){
                for(int yd = -1; yd < 2; ++yd){
                    for(int x = 0; x < Chunk.chunkSize; ++x){
                        for(int y = 0; y < Chunk.chunkSize; ++y){
                            ids[(xd + 1) * Chunk.chunkSize + x, (yd + 1) * Chunk.chunkSize] = chunks[xChunk + xd, yChunk + yd].rawIds[x,y];
                        }
                    }
                }
            }
            for(int x = 0; x < Chunk.chunkSize * 3; ++x){
                for(int y = 0; y < Chunk.chunkSize * 3; ++y){
                    if(ids[x,y] == tile.id){
                        terrainBrush.Paint(ids, tile.id, x, y);
                    }
                }
            }
            for(int x = 0; x < Chunk.chunkSize; ++x){
                for(int y = 0; y < Chunk.chunkSize; ++y){
                    if(ids[x + Chunk.chunkSize, y + Chunk.chunkSize] != tileManager.defaultTile.id) Debug.Log("hmmm");
                    chunks[xChunk, yChunk].grid[x, y].id = ids[x + Chunk.chunkSize, y + Chunk.chunkSize];
                }
            }
        }
    }

    void SmoothChunk(int[,] ids, int[,] savedIds, int xChunk, int yChunk){
        for(int x = 0; x < Chunk.chunkSize; ++x){
            for(int y = 0; y < Chunk.chunkSize; ++y){
                SmoothTile(ids, savedIds, x + xChunk * Chunk.chunkSize, y + yChunk * Chunk.chunkSize);
            }
        }
    }

    void SmoothTile(int[,] ids, int[,] savedIds, int x, int y){
        int tileId = savedIds[x, y];
        if(tileId == tileManager.defaultTile.id) return;
        int dir = TerrainBrush.Smooth(ids, tileManager.defaultTile.id, x, y);
        if(dir == (int) TerrainBrush.TileType.Error){
            ids[x,y] = tileManager.defaultTile.id;
            for(int xd = -1; xd < 2; ++xd){
                for(int yd = -1; yd < 2; ++yd){
                    if(ids[x + xd, y + yd] != tileManager.defaultTile.id){
                        SmoothTile(ids, savedIds, x + xd, y + yd);
                    }
                }
            }
        } else {
            ids[x,y] = tileId + dir;
        }
    }

    void MakeChunkSprites(Chunk chunk){
        for(int x = 0; x < Chunk.chunkSize; ++x){
            for(int y = 0; y < Chunk.chunkSize; ++y){
                TileInfo tile = tileManager.GetTile(chunk.grid[x,y].id);
                GameObject sprite = Instantiate(tileManager.tilePrefab, new Vector2(x + chunk.x * Chunk.chunkSize, 
                                                (y + chunk.y * Chunk.chunkSize)), Quaternion.identity);
                //sprite.transform.SetParent(transform);
                sprite.transform.localScale = new Vector2(1 / 0.16f, 1 / 0.16f);
                if(tile.isAnimation){
                    sprite.GetComponent<TileAnimator>().SetFrames(tile.sprites, tile.frameRate);
                } else {
                    sprite.GetComponent<SpriteRenderer>().sprite = tileManager.GetSprite(tile.id);
                }
                chunk.grid[x,y].tile = sprite;
            }
        }
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Tiles;

// public class WorldGenerator : MonoBehaviour
// {
//     public TileManager tileManager;
//     public double zoom = 16;
//     int seed;
//     List<TileInfo> terrainTiles;
//     List<TileInfo> resourceTiles;
//     public int smoothPadding;
//     public int renderedPadding;
//     public int smoothBrushSize;
//     PerlinNoiseGenerator generator;
//     TerrainBrush terrainBrush;

//     void Start()
//     {
//         generator = new PerlinNoiseGenerator();
//         seed = Mathf.Abs(System.Environment.TickCount / 10);
//         Debug.Log("Seed: " + seed);
//         Random.InitState(seed);
//         terrainBrush = new TerrainBrush(smoothBrushSize);

//         terrainTiles = new List<TileInfo>();
//         resourceTiles = new List<TileInfo>();
//         foreach(TileInfo tile in tileManager.tiles){
//             if(tile.isTerrain){
//                 terrainTiles.Add(tile);
//             }
//             if(tile.isResource){
//                 resourceTiles.Add(tile);
//             }
//         }
//         terrainTiles.Sort(TileInfo.ComparePriority);
//         resourceTiles.Sort(TileInfo.ComparePriority);
//     }

//     bool isContinuation(Chunk[,] chunks, int xChunk, int yChunk, int x, int y, int id){
//         for(int xd = -1; xd < 2; ++xd){
//             for(int yd = -1; yd < 2; ++yd){
//                 if(GetId(chunks, x + xd + xChunk * Chunk.chunkSize, y + yd + yChunk * Chunk.chunkSize) == id) return true;
//             }
//         }
//         return false;
//     }

//     bool NeighbourIsAnimation(Chunk[,] chunks, int xChunk, int yChunk, int x, int y, int id){
//         for(int xd = -1; xd < 2; ++xd){
//             for(int yd = -1; yd < 2; ++yd){
//                 int tileId = GetId(chunks, x + xd + xChunk * Chunk.chunkSize, y + yd + yChunk * Chunk.chunkSize);
//                 if(tileId == id || tileId == -1) continue;
//                 if(tileManager.GetTile(tileId).isAnimation) return true;
//             }
//         }
//         return false;
//     }

//     public int[,] CreateChunksArray(Chunk[,] chunks){
//         int[,] ids = new int[chunks.GetLength(0) * Chunk.chunkSize, chunks.GetLength(1) * Chunk.chunkSize];
//         for(int x = 0; x < chunks.GetLength(0); ++x){
//             for(int y = 0; y < chunks.GetLength(1); ++y){
//                 for(int xc = 0; xc < Chunk.chunkSize; ++xc){
//                     for(int yc = 0; yc < Chunk.chunkSize; ++yc){
//                         ids[x * Chunk.chunkSize + xc, y * Chunk.chunkSize + yc] = chunks[x,y].terrain[xc,yc].tileData.Id;
//                     }
//                 }
//             }
//         }
//         return ids;
//     }

//     public int GetId(Chunk[,] chunks, int x, int y){
//         int xChunk = x / Chunk.chunkSize;
//         int yChunk = y / Chunk.chunkSize;
//         int xc = x % Chunk.chunkSize;
//         int yc = y % Chunk.chunkSize;
//         if(xc < 0 || yc < 0) return -1;
//         if(xChunk >= chunks.GetLength(0) || yChunk >= chunks.GetLength(1)) return -1;
//         return chunks[xChunk, yChunk].terrain[xc, yc].tileData.Id;
//     }

//     public void LoadChunks(Chunk[,] chunks){
//         for(int x = 0; x < chunks.GetLength(0); ++x){
//             for(int y = 0; y < chunks.GetLength(1); ++y){
//                 if(chunks[x,y].chunkState == ChunkState.NotGenerated){
//                     for(int xc = 0; xc < Chunk.chunkSize; ++xc){
//                         for(int yc = 0; yc < Chunk.chunkSize; ++yc){
//                             chunks[x,y].terrain[xc,yc] = new Tile(TileData.AIR);
//                             chunks[x,y].tiles[xc,yc] = new Tile(TileData.AIR);
//                         }
//                     }
//                 }
//             }
//         }
//         for(int x = 0; x < chunks.GetLength(0); ++x){
//             for(int y = 0; y < chunks.GetLength(1); ++y){
//                 if(chunks[x,y].chunkState == ChunkState.NotGenerated){
//                     GenerateTerrain(chunks, x, y);
//                     GenerateResources(chunks[x,y]);
//                     chunks[x,y].chunkState = ChunkState.Saved;
//                 }
//             }
//         }
//         for(int x = renderedPadding; x < chunks.GetLength(0) - renderedPadding; ++x){
//             for(int y = renderedPadding; y < chunks.GetLength(1) - renderedPadding; ++y){
//                 if(chunks[x,y].chunkState == ChunkState.Saved){
//                     MakeTerrainSprites(chunks[x,y]);
//                     //MakeResourceSprites(chunks, x, y);
//                     chunks[x,y].chunkState = ChunkState.Rendered;
//                 }
//             }
//         }
//     }

//     double PerlinNoise(double x, double y, int id){
//         return generator.perlin((x + seed * id) / (zoom), (y + seed * id) / (zoom), seed / 10000000);
//     }

//     void GenerateTerrain(Chunk[,] chunks, int xChunk, int yChunk){
//         Chunk chunk = chunks[xChunk, yChunk];
//         // Then loop through each tile type except the default one
//         int i = 1;
//         foreach(TileInfo tileInfo in terrainTiles){
//             if(tileInfo.id == tileManager.defaultTile.id) continue;
//             for(int x = 0; x < Chunk.chunkSize; ++x){
//                 for(int y = 0; y < Chunk.chunkSize; ++y){
//                     // Check to see if the priority is already above current tile
//                     if(chunk.terrain[x,y].tileData != TileData.AIR) continue;
//                     if(NeighbourIsAnimation(chunks, xChunk, yChunk, x, y, tileInfo.id)) continue;
//                     double rand = PerlinNoise(x + chunk.x * Chunk.chunkSize, y + chunk.y * Chunk.chunkSize, i);
//                     if(isContinuation(chunks, xChunk, yChunk, x, y, tileInfo.id)){
//                         if(rand <= tileInfo.spawnChance + tileInfo.continuationBias){
//                             chunk.terrain[x,y].SetTileData(TileData.nameToData[tileInfo.tileName]);
//                         } else {
//                             chunk.terrain[x,y].SetTileData(TileData.nameToData[tileManager.defaultTileName]);
//                         }
//                     } else {
//                         // Otherwise check if perlin noise value is less than the spawn chance of the tile
//                         if(rand <= tileInfo.spawnChance){
//                             chunk.terrain[x,y].SetTileData(TileData.nameToData[tileInfo.tileName]);
//                         }
//                     }
//                 }
//             }
//             ++i;
//         }
//         for(int x = 0; x < Chunk.chunkSize; ++x){
//             for(int y = 0; y < Chunk.chunkSize; ++y){
//                 if(chunk.terrain[x,y].tileData == TileData.AIR){
//                     chunk.terrain[x,y].SetTileData(TileData.nameToData[tileManager.defaultTileName]);
//                 }  
//             }
//         }
//     }

//     void GenerateResources(Chunk chunk){
//         int i = terrainTiles.Count;
//         foreach(TileInfo tileInfo in resourceTiles){
//             int spawnTile = TileData.nameToData[tileInfo.resourceSpawnTile].Id;
//             for(int x = 0; x < Chunk.chunkSize; ++x){
//                 for(int y = 0; y < Chunk.chunkSize; ++y){
//                     if(chunk.terrain[x, y].tileData.Id != spawnTile) continue;
//                     if(chunk.tiles[x,y].tileData.Id != 0) continue;
//                     double rand = Random.value;
//                     if(rand < tileInfo.singleResourceChance){
//                         chunk.tiles[x,y].SetTileData(TileData.nameToData[tileInfo.tileName]);
//                     } else {
//                         rand = PerlinNoise(x + chunk.x * Chunk.chunkSize, y + chunk.y * Chunk.chunkSize, i);
//                         if(rand < tileInfo.veinChance){
//                             chunk.tiles[x,y].SetTileData(TileData.nameToData[tileInfo.tileName]);
//                         }
//                     }
//                 }
//             }
//         }
//     }

//     void SmoothChunk(int[,] ids, int[,] savedIds, int xChunk, int yChunk){
//         TerrainBrush terrainBrush = new TerrainBrush(smoothBrushSize);
//         for(int x = 0; x < Chunk.chunkSize; ++x){
//             for(int y = 0; y < Chunk.chunkSize; ++y){
//                 SmoothTile(ids, savedIds, x + xChunk * Chunk.chunkSize, y + yChunk * Chunk.chunkSize);
//             }
//         }
//     }

//     void SmoothTile(int[,] ids, int[,] savedIds, int x, int y){
//         int tileId = savedIds[x, y];
//         if(tileId == tileManager.defaultTile.id) return;
//         int dir = terrainBrush.Smooth(ids, tileManager.defaultTile.id, x, y);
//         if(dir == (int) TerrainBrush.TileType.Error){
//             ids[x,y] = tileManager.defaultTile.id;
//             for(int xd = -1; xd < 2; ++xd){
//                 for(int yd = -1; yd < 2; ++yd){
//                     if(ids[x + xd, y + yd] != tileManager.defaultTile.id){
//                         SmoothTile(ids, savedIds, x + xd, y + yd);
//                     }
//                 }
//             }
//         } else {
//             ids[x,y] = tileId + dir;
//         }
//     }

//     void MakeTerrainSprites(Chunk chunk){
//         for(int x = 0; x < Chunk.chunkSize; ++x){
//             for(int y = 0; y < Chunk.chunkSize; ++y){
//                 chunk.terrain[x, y].CreateTileObject(new Vector2(x + chunk.x * Chunk.chunkSize, (y + chunk.y * Chunk.chunkSize)));
//                 chunk.tiles[x, y].CreateTileObject(new Vector2(x + chunk.x * Chunk.chunkSize, (y + chunk.y * Chunk.chunkSize)));
//             }
//         }
//     }
// }
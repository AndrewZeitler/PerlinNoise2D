using UnityEngine;

public class TerrainBrush {
    int size;

    public enum TileType{
        Center = 0,
        Top,
        Right,
        Bottom,
        Left,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        TopLeftL,
        TopRightL,
        BottomLeftL,
        BottomRightL,
        LeftDiagonal,
        RightDiagonal,
        Error = 100
    }

    public TerrainBrush(int size){
        this.size = size;
    }

    // public int[,] Paint(int[,] ids, int currId, int x, int y){
    //     for(int xd = -size / 2; xd < size / 2 + 1; ++xd){
    //         for(int yd = -size / 2; yd < size / 2 + 1; ++yd){
    //             if(!isInBounds(x + xd, y + yd, ids.GetLength(0), ids.GetLength(1))) continue;
    //             ids[x + xd, y + yd] = currId + (int) GetTileType(ids, currId, x + xd, y + yd);
    //         }
    //     }
    //     return ids;
    // }

    public void Paint(Chunk chunk, int currId, int x, int y){
        for(int xd = -size / 2; xd < size / 2 + 1; ++xd){
            for(int yd = -size / 2; yd < size / 2 + 1; ++yd){
                if(xd * xd + yd * yd < size * size) {
                    if(x + xd < 0 || x + xd >= Chunk.chunkSize || y + yd < 0 || y + yd >= Chunk.chunkSize) continue;
                    chunk.grid[x + xd, y + yd].id = currId;
                }
            }
        }
    }

    public int Smooth(int[,] ids, int currId, int x, int y){
        return (int) GetTileType(ids, currId, x, y);
    }

    TileType GetTileType(int[,] ids, int currId, int x, int y){
        if(ids[x, y - 1] == currId && ids[x, y + 1] == currId) return TileType.Error;
        if(ids[x - 1, y] == currId && ids[x + 1, y] == currId) return TileType.Error;
        if(ids[x, y - 1] == currId){
            if(ids[x - 1, y] == currId) return TileType.TopRight;
            else if(ids[x + 1, y] == currId) return TileType.TopLeft;
            return TileType.Top;
        } else if(ids[x, y + 1] == currId){
            if(ids[x - 1, y] == currId) return TileType.BottomRight;
            else if(ids[x + 1, y] == currId) return TileType.BottomLeft;
            return TileType.Bottom;
        } else if(ids[x - 1, y] == currId){
            return TileType.Right;
        } else if(ids[x + 1, y] == currId){
            return TileType.Left;
        } else if(ids[x - 1, y - 1] == currId){
            if(ids[x + 1, y + 1] == currId) return TileType.RightDiagonal;
            return TileType.TopRightL;
        } else if(ids[x + 1, y - 1] == currId){
            if(ids[x - 1, y + 1] == currId) return TileType.LeftDiagonal;
            return TileType.TopLeftL;
        } else if(ids[x - 1, y + 1] == currId){
            return TileType.BottomRightL;
        } else if(ids[x + 1, y + 1] == currId){
            return TileType.BottomLeftL;
        }
        return TileType.Center;
    }

    bool isInBounds(int x, int y, int xLength, int yLength){
        return !(x < 0 || x >= xLength || y < 0 || y >= yLength);
    }

    // TileType GetTileType(int[,] ids, int currId, int x, int y){
    //     int idCount = 0;
    //     if(ids[x,y] == currId) return TileType.Center;
    //     for(int xd = -size / 2; xd < size / 2 + 1; ++xd){
    //         for(int yd = -size / 2; yd < size / 2 + 1; ++yd){
    //             if(ids[x + xd, y + yd] == currId) ++idCount;
    //         }
    //     }
    //     if((ids[x - 1, y] == currId && ids[x + 1, y] == currId) || (ids[x, y - 1] == currId && ids[x, y + 1] == currId)) return TileType.Center;
    //     if(idCount == 8 || idCount == 7 || idCount == 6) {
    //         return TileType.Center;
    //     } else if(idCount == 5) {
    //         if(ids[x - 1, y] == currId && ids[x, y - 1] == currId) return TileType.BottomLeftL;
    //         if(ids[x - 1, y] == currId && ids[x, y + 1] == currId) return TileType.TopLeftL;
    //         if(ids[x + 1, y] == currId && ids[x, y - 1] == currId) return TileType.BottomRightL;
    //         if(ids[x + 1, y] == currId && ids[x, y + 1] == currId) return TileType.TopRightL;
    //     } else if(idCount == 4){
    //         if(ids[x - 1, y] == currId && ids[x, y - 1] == currId) return TileType.BottomLeftL;
    //         if(ids[x - 1, y] == currId && ids[x, y + 1] == currId) return TileType.TopLeftL;
    //         if(ids[x + 1, y] == currId && ids[x, y - 1] == currId) return TileType.BottomRightL;
    //         if(ids[x + 1, y] == currId && ids[x, y + 1] == currId) return TileType.TopRightL;
    //     } else if(idCount == 3 || idCount == 2){
    //         if(ids[x - 1, y - 1] == currId){
    //             if(ids[x + 1, y] == currId) return TileType.BottomRightL;
    //             else if(ids[x + 1, y + 1] == currId) return TileType.LeftDiagonal;
    //             else if(ids[x, y + 1] == currId) return TileType.TopLeftL;
    //             else if(ids[x, y - 1] == currId && ids[x - 1, y] == currId) return TileType.BottomLeftL;
    //         } else if(ids[x + 1, y - 1] == currId){
    //             if(ids[x - 1, y] == currId) return TileType.BottomLeftL;
    //             else if(ids[x - 1, y + 1] == currId) return TileType.RightDiagonal;
    //             else if(ids[x, y + 1] == currId) return TileType.TopRightL;
    //             else if(ids[x, y - 1] == currId && ids[x + 1, y] == currId) return TileType.BottomRightL;
    //         } else if(ids[x - 1, y + 1] == currId){
    //             if(ids[x, y - 1] == currId) return TileType.BottomLeftL;
    //             else if(ids[x + 1, y - 1] == currId) return TileType.RightDiagonal;
    //             else if(ids[x + 1, y] == currId) return TileType.TopRightL;
    //             else if(ids[x, y + 1] == currId && ids[x - 1, y] == currId) return TileType.TopLeftL;
    //         } else if(ids[x + 1, y + 1] == currId){
    //             if(ids[x, y - 1] == currId) return TileType.BottomRightL;
    //             else if(ids[x - 1, y - 1] == currId) return TileType.LeftDiagonal;
    //             else if(ids[x - 1, y] == currId) return TileType.TopLeftL;
    //             else if(ids[x, y + 1] == currId && ids[x + 1, y] == currId) return TileType.TopRightL;
    //         }
    //         if(ids[x, y - 1] == currId) return TileType.Bottom;
    //         if(ids[x - 1, y] == currId) return TileType.Left;
    //         if(ids[x + 1, y] == currId) return TileType.Right;
    //         if(ids[x, y + 1] == currId) return TileType.Top;
    //     } else if(idCount == 1){
    //         if(ids[x - 1, y - 1] == currId) return TileType.BottomLeft;
    //         if(ids[x, y - 1] == currId) return TileType.Bottom;
    //         if(ids[x + 1, y - 1] == currId) return TileType.BottomRight;
    //         if(ids[x - 1, y] == currId) return TileType.Left;
    //         if(ids[x + 1, y] == currId) return TileType.Right;
    //         if(ids[x - 1, y + 1] == currId) return TileType.TopLeft;
    //         if(ids[x, y + 1] == currId) return TileType.Top;
    //         if(ids[x + 1, y + 1] == currId) return TileType.TopRight;
    //     }
    //     return TileType.Center;
    // }
}
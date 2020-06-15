using UnityEngine;

public class TerrainBrush {
    int size;

    public enum TileType{
        BottomRightL = 0,
        BottomLeftL = 1,
        Bottom = 2,
        TopRightL = 3,
        Right = 4,
        RightDiagonal = 5,
        BottomRight = 6,
        TopLeftL = 7,
        LeftDiagonal = 8,
        Left = 9,
        BottomLeft = 10,
        Top = 11,
        TopRight = 12,
        TopLeft = 13,
        Center = 14,
        Error = 100
    }

    public TerrainBrush(int size){
        this.size = size;
    }

    // public void Paint(Chunk chunk, int currId, int x, int y){
    //     for(int xd = -size / 2; xd < size / 2 + 1; ++xd){
    //         for(int yd = -size / 2; yd < size / 2 + 1; ++yd){
    //             if(xd * xd + yd * yd < size * size) {
    //                 if(x + xd < 0 || x + xd >= Chunk.chunkSize || y + yd < 0 || y + yd >= Chunk.chunkSize) continue;
    //                 chunk.terrain[x + xd, y + yd].tileData.Id = currId;
    //             }
    //         }
    //     }
    // }

    // public int Smooth(int[,] ids, int currId, int x, int y){
    //     return (int) GetTileType(ids, currId, x, y);
    // }

    // public static TileType GetTileType(int[,] ids, int currId, int x, int y){
    //     if(ids[x, y - 1] == currId && ids[x, y + 1] == currId) return TileType.Error;
    //     if(ids[x - 1, y] == currId && ids[x + 1, y] == currId) return TileType.Error;
    //     if(ids[x, y - 1] == currId){
    //         if(ids[x - 1, y] == currId) return TileType.TopRight;
    //         else if(ids[x + 1, y] == currId) return TileType.TopLeft;
    //         return TileType.Top;
    //     } else if(ids[x, y + 1] == currId){
    //         if(ids[x - 1, y] == currId) return TileType.BottomRight;
    //         else if(ids[x + 1, y] == currId) return TileType.BottomLeft;
    //         return TileType.Bottom;
    //     } else if(ids[x - 1, y] == currId){
    //         return TileType.Right;
    //     } else if(ids[x + 1, y] == currId){
    //         return TileType.Left;
    //     } else if(ids[x - 1, y - 1] == currId){
    //         if(ids[x + 1, y + 1] == currId) return TileType.RightDiagonal;
    //         return TileType.TopRightL;
    //     } else if(ids[x + 1, y - 1] == currId){
    //         if(ids[x - 1, y + 1] == currId) return TileType.LeftDiagonal;
    //         return TileType.TopLeftL;
    //     } else if(ids[x - 1, y + 1] == currId){
    //         return TileType.BottomRightL;
    //     } else if(ids[x + 1, y + 1] == currId){
    //         return TileType.BottomLeftL;
    //     }
    //     return TileType.Center;
    // }

    public static TileType GetTileType(int[,] ids, int x, int y){
        int currId = ids[x, y];
        if(ids[x, y - 1] != currId && ids[x, y + 1] != currId) return TileType.Error;
        if(ids[x - 1, y] != currId && ids[x + 1, y] != currId) return TileType.Error;
        if(ids[x, y - 1] != currId){
            if(ids[x - 1, y] != currId) return TileType.TopRight;
            else if(ids[x + 1, y] != currId) return TileType.TopLeft;
            return TileType.Top;
        } else if(ids[x, y + 1] != currId){
            if(ids[x - 1, y] != currId) return TileType.BottomRight;
            else if(ids[x + 1, y] != currId) return TileType.BottomLeft;
            return TileType.Bottom;
        } else if(ids[x - 1, y] != currId){
            return TileType.Right;
        } else if(ids[x + 1, y] != currId){
            return TileType.Left;
        } else if(ids[x - 1, y - 1] != currId){
            if(ids[x + 1, y + 1] != currId) return TileType.RightDiagonal;
            return TileType.TopRightL;
        } else if(ids[x + 1, y - 1] != currId){
            if(ids[x - 1, y + 1] != currId) return TileType.LeftDiagonal;
            return TileType.TopLeftL;
        } else if(ids[x - 1, y + 1] != currId){
            return TileType.BottomRightL;
        } else if(ids[x + 1, y + 1] != currId){
            return TileType.BottomLeftL;
        }
        return TileType.Center;
    }

    public static int GetOtherType(int[,] ids, int x, int y, TileType type){
        if(type == TileType.BottomRightL || type == TileType.BottomRight){
                return ids[x - 1, y + 1];
        } else if(type == TileType.BottomLeftL || type == TileType.BottomLeft){
                return ids[x + 1, y + 1];
        } else if(type == TileType.Bottom){
                return ids[x, y + 1];
        } else if(type == TileType.TopRightL || type == TileType.TopRight || type == TileType.RightDiagonal){
                return ids[x - 1, y - 1];
        } else if(type == TileType.Right){
                return ids[x - 1, y];
        } else if(type == TileType.TopLeftL || type == TileType.TopLeft || type == TileType.LeftDiagonal){
            return ids[x + 1, y - 1];
        } else if(type == TileType.Left){
            return ids[x + 1, y];
        } else if(type == TileType.Top){
            return ids[x, y - 1];
        } else if(type == TileType.Error){
            for(int xd = -1; xd < 2; ++xd){
                for(int yd = -1; yd < 2; ++yd){
                    if(ids[x + xd, y + yd] != ids[x, y]) return ids[x + xd, y + yd];
                }
            }
        }
        return -1;
    }

    bool isInBounds(int x, int y, int xLength, int yLength){
        return !(x < 0 || x >= xLength || y < 0 || y >= yLength);
    }
}
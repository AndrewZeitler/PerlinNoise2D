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

    public void Paint(Chunk chunk, int currId, int x, int y){
        for(int xd = -size / 2; xd < size / 2 + 1; ++xd){
            for(int yd = -size / 2; yd < size / 2 + 1; ++yd){
                if(xd * xd + yd * yd < size * size) {
                    if(x + xd < 0 || x + xd >= Chunk.chunkSize || y + yd < 0 || y + yd >= Chunk.chunkSize) continue;
                    chunk.terrain[x + xd, y + yd].id = currId;
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
}
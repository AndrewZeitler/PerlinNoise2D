
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
        RightDiagonal
    }

    public TerrainBrush(int size){
        this.size = size;
    }

    public void Paint(int[,] ids, int currId, int x, int y){
        for(int xd = -size / 2; xd < size / 2 + 1; ++xd){
            for(int yd = -size / 2; yd < size / 2 + 1; ++yd){
                ids[x + xd, y + yd] = currId + (int) GetTileType(ids, currId, x + xd, y + yd);
            }
        }
    }

    TileType GetTileType(int[,] ids, int currId, int x, int y){
        int idCount = 0;
        for(int xd = -size / 2; xd < size / 2 + 1; ++xd){
            for(int yd = -size / 2; yd < size / 2 + 1; ++yd){
                if(ids[x + xd, y + yd] == currId) ++idCount;
            }
        }
        if(idCount == 8 || idCount == 7 || idCount == 6) return TileType.Center;
        if(idCount == 5) {
            if(ids[x - 1, y] == currId && ids[x, y - 1] == currId) return TileType.TopLeftL;
            if(ids[x - 1, y] == currId && ids[x, y + 1] == currId) return TileType.BottomLeftL;
            if(ids[x + 1, y] == currId && ids[x, y - 1] == currId) return TileType.TopRightL;
            return TileType.BottomRightL;
        }
        if(idCount == 4){
            if(ids[x - 1, y] == currId && ids[x, y - 1] == currId) return TileType.TopLeftL;
            if(ids[x - 1, y] == currId && ids[x, y + 1] == currId) return TileType.BottomLeftL;
            if(ids[x + 1, y] == currId && ids[x, y - 1] == currId) return TileType.TopRightL;
            return TileType.BottomRightL;
        }
        if(idCount == 3 || idCount == 2){
            if(ids[x - 1, y - 1] == currId){
                if(ids[x + 1, y] == currId) return TileType.TopRightL;
                else if(ids[x + 1, y + 1] == currId) return TileType.RightDiagonal;
                return TileType.BottomLeftL;
            } else if(ids[x + 1, y - 1] == currId){
                if(ids[x - 1, y] == currId) return TileType.TopLeftL;
                else if(ids[x - 1, y + 1] == currId) return TileType.LeftDiagonal;
                return TileType.BottomRightL;
            } else if(ids[x - 1, y + 1] == currId){
                if(ids[x, y - 1] == currId) return TileType.TopLeftL;
                else if(ids[x + 1, y - 1] == currId) return TileType.LeftDiagonal;
                return TileType.BottomRightL;
            } else if(ids[x + 1, y + 1] == currId){
                if(ids[x, y - 1] == currId) return TileType.TopRightL;
                else if(ids[x - 1, y - 1] == currId) return TileType.RightDiagonal;
                return TileType.BottomLeftL;
            }
            if(ids[x, y - 1] == currId) return TileType.Top;
            if(ids[x - 1, y] == currId) return TileType.Left;
            if(ids[x + 1, y] == currId) return TileType.Right;
            return TileType.Bottom;
        }
        if(idCount == 1){
            if(ids[x - 1, y - 1] == currId) return TileType.TopLeft;
            if(ids[x, y - 1] == currId) return TileType.Top;
            if(ids[x + 1, y - 1] == currId) return TileType.TopRight;
            if(ids[x - 1, y] == currId) return TileType.Left;
            if(ids[x + 1, y] == currId) return TileType.Right;
            if(ids[x - 1, y + 1] == currId) return TileType.BottomLeft;
            if(ids[x, y + 1] == currId) return TileType.Bottom;
            return TileType.BottomRight;
        }
        return TileType.Center;
    }
}
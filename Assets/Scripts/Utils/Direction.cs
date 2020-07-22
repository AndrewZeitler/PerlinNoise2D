using UnityEngine;
using World;

namespace Utils {

    public enum Direction {
        NW,
        N,
        NE,
        W,
        C,
        E,
        SW,
        S,
        SE
    }

    public static class ChunkDirection {

        public static Direction GetDirection(int x, int y){
            int cq = Chunk.chunkSize / 4;
            if(x < cq) {
                if(y < cq) return Direction.NW;
                else if(y >= 3 * cq) return Direction.SW;
                return Direction.W;
            } else if(x >= 3 * cq){
                if(y < cq) return Direction.NE;
                else if(y >= 3 * cq) return Direction.SE;
                return Direction.E;
            }
            if(y < cq) return Direction.N;
            if(y >= 3 * cq) return Direction.S;
            return Direction.C;
        }

        public static Vector2Int ChangeCoordinate(Direction direction, Vector2Int coords, int delta){
            switch(direction){
                case Direction.NW: return new Vector2Int(coords.x - delta, coords.y - delta);
                case Direction.N: return new Vector2Int(coords.x, coords.y - delta);
                case Direction.NE: return new Vector2Int(coords.x + delta, coords.y - delta);
                case Direction.W: return new Vector2Int(coords.x - delta, coords.y);
                case Direction.E: return new Vector2Int(coords.x + delta, coords.y);
                case Direction.SW: return new Vector2Int(coords.x - delta, coords.y + delta);
                case Direction.S: return new Vector2Int(coords.x, coords.y + delta);
                case Direction.SE: return new Vector2Int(coords.x + delta, coords.y + delta);
                default: return coords;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles {
    public class TileData : TileEnum {
        
        public static Dictionary<int, TileData> idToData = new Dictionary<int, TileData>();
        public static Dictionary<string, TileData> nameToData = new Dictionary<string, TileData>();

        public static readonly TileData AIR = new TileData("air", 0, new TileModifier[]{});
        public static readonly TileData GRASS = new TileData("grass", 1, new TileModifier[]{new TileRenderer(12)});
        public static readonly TileData DIRT = new TileData("dirt", 2, new TileModifier[]{new AutoTile()});
        public static readonly TileData STONE = new TileData("stone", 3, new TileModifier[]{new AutoTile()});
        public static readonly TileData WATER = new TileData("water", 4, new TileModifier[]{new AutoTileAnimated(3, 0.3f)}, false);
        public static readonly TileData TREE = new TileData("tree", 5, new TileModifier[]{new TileRenderer(3), new TileOrderer()}, false);
        public static readonly TileData DIRT_ROCK = new TileData("dirtRock", 6, new TileModifier[]{new TileRenderer(7), new TileOrderer()});
        public static readonly TileData STONE_ROCK = new TileData("stoneRock", 7, new TileModifier[]{new TileRenderer(7), new TileOrderer()});
        public static readonly TileData LILYPAD = new TileData("lilypad", 8, new TileModifier[]{new TileRenderer(4), new TileOrderer()});

        public TileData(string name, int id, TileModifier[] tileModifiers, bool isWalkable = true) : base(name, id, tileModifiers, isWalkable) {
            idToData.Add(id, this);
            nameToData.Add(name, this);
        }

        public TileModifier GetModifierOfType<T>(){
            foreach(TileModifier modifier in TileModifiers){
                if(modifier.GetType() == typeof(T)) return modifier;
            }
            return null;
        }
    }
}
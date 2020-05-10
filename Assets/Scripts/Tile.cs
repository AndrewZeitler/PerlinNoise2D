using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
    public int id;
    public GameObject tile;
    public bool isWalkable;
    public bool hasCollider;

    public Tile(int id, bool isWalkable){ 
        this.id = id;
        tile = null;
        this.isWalkable = isWalkable;
        hasCollider = false;
    }

    public void DestroyTile(){
        Object.Destroy(tile);
    }
}
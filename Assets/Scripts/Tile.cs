using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
    public int id;
    public GameObject tile;

    public Tile(int id){ 
        this.id = id;
        tile = null;
    }

    public void DestroyTile(){
        Object.Destroy(tile);
    }
}
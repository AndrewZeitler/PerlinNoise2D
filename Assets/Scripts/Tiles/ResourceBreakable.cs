using UnityEngine;
using Items;
using Entities;

namespace Tiles {
    
    public class ResourceBreakable : TileModifier {
        LootTable lootTable;
        float waitTime;
        ItemData breakTool;
        bool breaking = false;
        Entity breaker = null;

        public ResourceBreakable(LootTable lootTable, float waitTime, ItemData breakTool){
            this.lootTable = lootTable;
            this.waitTime = waitTime;
            this.breakTool = breakTool;
        }

        public override void Initialize(Tile tile){
            this.tile = tile;
            tile.AddInteractListener(OnInteract);
            tile.AddReleaseInteractListener(ReleaseInteract);
        }

        public void OnInteract(Entity entity){
            Debug.Log("Interact!");
            Player player = entity as Player; 
            if(player != null){
                if(player.GetHeldItem() != null && player.GetHeldItem().GetItem().itemData == breakTool){
                    breaking = true;
                    breaker = entity;
                    tile.tileScript.StartCoroutine(Interacting());
                }
            }
        }

        public void ReleaseInteract(Entity entity){
            Debug.Log("No Interact!");
            breaking = false;
        }

        public System.Collections.IEnumerator Interacting() {
            while(breaking){
                yield return new WaitForSeconds(waitTime);
                if(breaking) breaker.AddItem(lootTable.GetDrop());
            }
        }
    }

}
using UnityEngine;
using Items;
using Entities;
using Utils;

namespace Tiles {
    
    public class ResourceBreakable : TileModifier {
        LootTable lootTable;
        float waitTime;
        ItemData breakTool;
        Entity breaker = null;
        Coroutine coroutine = null;
        int resourceAmount;
        ItemData itemMain;

        public ResourceBreakable(LootTable lootTable, float waitTime, ItemData breakTool, Range resourceRange, ItemData itemMain){
            this.lootTable = lootTable;
            this.waitTime = waitTime;
            this.breakTool = breakTool;
            resourceAmount = (int) Random.Range(resourceRange.min, resourceRange.max);
            this.itemMain = itemMain;
        }

        public override void Initialize(Tile tile){
            this.tile = tile;
            tile.AddInteractListener(OnInteract);
            tile.AddReleaseInteractListener(ReleaseInteract);
        }

        public void OnInteract(Entity entity){
            if(coroutine != null) return;
            Player player = entity as Player; 
            if(player != null){
                if(player.GetHeldItem() != null && player.GetHeldItem().GetItem().itemData == breakTool){
                    breaker = entity;
                    coroutine = tile.tileScript.StartCoroutine(Interacting());
                }
            }
        }

        public void ReleaseInteract(Entity entity){
            if(tile == null || coroutine == null) return;
            tile.tileScript.StopCoroutine(coroutine);
            coroutine = null;
        }

        public System.Collections.IEnumerator Interacting() {
            while(true){
                yield return new WaitForSeconds(waitTime);
                ItemStack drop = lootTable.GetDrop();
                if(drop.GetItem().itemData == itemMain){
                    int dropAmount = Mathf.Min(resourceAmount, drop.GetAmount());
                    resourceAmount -= dropAmount;
                    breaker.AddItem(new ItemStack(itemMain, dropAmount));
                    if(resourceAmount == 0) {
                        tile.SetTileData(TileData.AIR);
                    }
                } else {
                    breaker.AddItem(drop);
                }
            }
        }
    }

}
using UnityEngine;
using World;
using Entities;
using Tiles;


namespace Items {
    public class Placeable : ItemModifier {
        public string tileName;
        public TileData tile;

        public Placeable(string tileName){
            this.tileName = tileName;
        }

        public override void Initialize(Item item) {
            tile = TileData.nameToData[tileName];
            this.item = item;
            item.AddRightClickListener(PlaceItem);
        }

        public void PlaceItem(Entity entity, Vector3 mouse){
            Player player = entity as Player;
            if(player != null){
                Vector2 world = WorldManager.MouseToWorld(mouse);
                Tile tile = WorldManager.GetTile(world);
                if(tile.tileData != TileData.AIR) return;
                Tile terrain = WorldManager.GetTerrain(world);
                if(!terrain.tileData.IsWalkable) return;
                ItemStack item = player.GetHeldItem();
                if(!player.inventory.RemoveItemAt(player.selectedSlot, 1)) player.inventory.RemoveItemAt(player.selectedSlot);
                tile.SetTileData(this.tile);
            }
        }
    }
}
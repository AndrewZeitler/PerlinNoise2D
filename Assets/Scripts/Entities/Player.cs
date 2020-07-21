using UnityEngine;
using Items;
using Events;
using UnityEngine.Events;
using World;
using Tiles;

namespace Entities {

    public class Player : Entity {
        public static readonly int INVENTORY_SIZE = 28;
        public static readonly int HOTBAR_SIZE = 8;
        public static readonly float RANGE = 3.5f;
        PlayerController controller;
        public int selectedSlot { get; private set; }
        SlotEvent slotEvent;

        bool holdingAttack = false;
        bool holdingInteract = false;

        Tile previousAttackTile;
        Tile previousInteractTile;
        Tile previousHoverTile;

        Item leftClickItem = null;
        Item rightClickItem = null;

        public Player(string name, Vector2 pos) : base(name) {
            slotEvent = new SlotEvent();
            selectedSlot = 0;
            GameObject prefab = Resources.Load("Prefabs/" + name) as GameObject;
            inventory = new Inventory(INVENTORY_SIZE + HOTBAR_SIZE, 4, "Inventory");
            inventory.AddItem(new ItemStack(ItemData.PICKAXE, 1));
            inventory.AddItem(new ItemStack(ItemData.SHOVEL, 1));
            inventory.AddItem(new ItemStack(ItemData.WOOD, 75));
            inventory.AddItem(new ItemStack(ItemData.WORKBENCH, 10));
            entityObject = PlayerController.Instantiate(prefab, pos, Quaternion.identity);
            controller = entityObject.GetComponent<PlayerController>();

            controller.AddSlotListener(HeldItemChangeEvent);
            controller.AddOnAttackListener(OnAttack);
            controller.AddOnInteractListener(OnInteract);
            controller.AddReleaseAttackListener(ReleaseAttack);
            controller.AddReleaseInteractListener(ReleaseInteract);
            controller.AddMouseMoveEvent(MouseChangePosition);
        }

        public ItemStack GetHeldItem(){
            return inventory.GetItemStackAt(selectedSlot);
        }

        public void AddSlotListener(UnityAction<int> listener){
            slotEvent.AddListener(listener);
        }

        void HeldItemChangeEvent(int delta){
            selectedSlot += delta;
            if(selectedSlot < 0) selectedSlot = HOTBAR_SIZE - 1;
            if(selectedSlot >= HOTBAR_SIZE) selectedSlot = 0;
            slotEvent.Invoke(selectedSlot);
        }

        public void SetSelectedSlot(int slot){
            if(slot < 0 || slot >= HOTBAR_SIZE) return;
            selectedSlot = slot;
            slotEvent.Invoke(selectedSlot);
        }

        // REWORK ITEM USE SYSTEM!!!!!!!!!!!!!!!!!!!!

        public void OnAttack(Vector3 mouse){
            Tile tile = WorldManager.MouseToTile(mouse, this);
            if(previousAttackTile != null && tile != previousAttackTile){
                previousAttackTile.ReleaseAttackEvent(this);
                previousAttackTile = null;
                return;
            }
            if(tile != null) {
                tile.OnAttackEvent(this);
                if(!tile.CancelItemUse && leftClickItem == null && GetHeldItem() != null){
                    leftClickItem = GetHeldItem().GetItem();
                    GetHeldItem().GetItem().OnLeftClickEvent(this, mouse);
                }
            }
            previousAttackTile = tile;
            holdingAttack = true;
        }

        public void OnInteract(Vector3 mouse){
            Tile tile = WorldManager.MouseToTile(mouse, this);
            if(previousInteractTile != null && tile != previousInteractTile){
                previousInteractTile.ReleaseInteractEvent(this);
                previousInteractTile = null;
                return;
            }
            if(tile != null) {
                tile.OnInteractEvent(this);
                if(!tile.CancelItemUse && rightClickItem == null && GetHeldItem() != null){
                    rightClickItem = GetHeldItem().GetItem();
                    GetHeldItem().GetItem().OnRightClickEvent(this, mouse);
                }
            }
            previousInteractTile = tile;
            holdingInteract = true;
        }

        public void ReleaseAttack(Vector3 mouse){
            if(previousAttackTile != null) {
                previousAttackTile.ReleaseAttackEvent(this);
                if(!previousAttackTile.CancelItemUse && leftClickItem != null && GetHeldItem() != null){
                    GetHeldItem().GetItem().ReleaseLeftClickEvent(this, mouse);
                    leftClickItem = null;
                }
            }
            previousAttackTile = null;
            holdingAttack = false;
        }

        public void ReleaseInteract(Vector3 mouse){
            if(previousInteractTile != null) {
                previousInteractTile.ReleaseInteractEvent(this);
                if(!previousInteractTile.CancelItemUse && rightClickItem != null && GetHeldItem() != null){
                    GetHeldItem().GetItem().ReleaseRightClickEvent(this, mouse);
                    rightClickItem = null;
                }
            }
            previousInteractTile = null;
            holdingInteract = false;
        }

        public void MouseChangePosition(Vector3 mouse){
            if(holdingAttack) OnAttack(mouse);
            if(holdingInteract) OnInteract(mouse);

            Tile tile = WorldManager.MouseToTile(mouse, this);
            if(tile != previousHoverTile){
                if(previousHoverTile != null) previousHoverTile.DeleteOutline();
                if(tile != null) tile.CreateOutline();
                previousHoverTile = tile;
            }
        }
    }

}
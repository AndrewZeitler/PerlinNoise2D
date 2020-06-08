using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Events;

namespace Tiles {
    public class Tile {
        public TileData tileData { get; private set; }
        public GameObject gameObject { get; private set; }
        public SpriteRenderer spriteRenderer { get; private set; }
        public TileScript tileScript { get; private set; }
        public bool hasCollider;
        Sprite[] sprites;
        TileModifier[] modifiers;

        UnityEvent OnDataChange;
        UnityEvent OnObjectCreated;
        PlayerEvent OnAttack;
        PlayerEvent OnInteract;

        public Tile(TileData tileData){
            OnObjectCreated = new UnityEvent();
            OnDataChange = new UnityEvent();
            OnAttack = new PlayerEvent();
            OnInteract = new PlayerEvent();
            this.tileData = tileData;
            hasCollider = false;
            InitializeModifiers();
        }

        public void InitializeModifiers(){
            modifiers = new TileModifier[tileData.TileModifiers.Length];
            for(int i = 0; i < modifiers.Length; ++i){
                modifiers[i] = tileData.TileModifiers[i].Clone();
                modifiers[i].Initialize(this);
            }
        }

        public void SetTileData(TileData tileData){
            this.tileData = tileData;
            if(gameObject != null) {
                Vector2 pos = gameObject.transform.position;
                if(gameObject == null) return;
                foreach(TileModifier modifier in modifiers){
                    modifier.Destroy();
                }
                OnObjectCreated.RemoveAllListeners();
                OnDataChange.RemoveAllListeners();
                OnAttack.RemoveAllListeners();
                OnInteract.RemoveAllListeners();
                DestroyTile();
                InitializeModifiers();
                CreateTileObject(pos);
            } else {
                InitializeModifiers();
            }
            OnDataChange.Invoke();
        }

        public void CreateTileObject(Vector2 pos){
            gameObject = GameObject.Instantiate(new GameObject(), new Vector3(pos.x, pos.y, pos.y), Quaternion.identity);
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            tileScript = gameObject.AddComponent<TileScript>();
            spriteRenderer.sortingLayerName = "Terrain";
            OnObjectCreated.Invoke();
        }

        public void AddCreateListener(UnityAction listener){
            OnObjectCreated.AddListener(listener);
        }

        public void AddDataChangeListener(UnityAction listener){
            OnDataChange.AddListener(listener);
        }

        public void AddAttackListener(UnityAction<PlayerController> listener){
            OnAttack.AddListener(listener);
        }

        public void AddInteractListener(UnityAction<PlayerController> listener){
            OnInteract.AddListener(listener);
        }

        public void OnAttackEvent(PlayerController player){
            OnAttack.Invoke(player);
        }

        public void OnInteractEvent(PlayerController player){

        }

        public void DestroyTile(){
            Object.Destroy(gameObject);
        }
    }
}
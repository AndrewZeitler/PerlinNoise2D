using UnityEngine;
using UnityEngine.Events;
using Events;
using Entities;

namespace Tiles {
    public class Tile {
        public TileData tileData { get; private set; }
        public GameObject gameObject { get; private set; }
        public SpriteRenderer spriteRenderer { get; private set; }
        public TileScript tileScript { get; private set; }
        public bool hasCollider;
        TileModifier[] modifiers;

        UnityEvent OnDataChange;
        UnityEvent OnObjectCreated;
        UnityEvent OnObjectDestroyed;
        EntityEvent OnAttack;
        EntityEvent OnInteract;
        EntityEvent ReleaseAttack;
        EntityEvent ReleaseInteract;

        public Tile(TileData tileData){
            OnObjectCreated = new UnityEvent();
            OnObjectDestroyed = new UnityEvent();
            OnDataChange = new UnityEvent();
            OnAttack = new EntityEvent();
            OnInteract = new EntityEvent();
            ReleaseAttack = new EntityEvent();
            ReleaseInteract = new EntityEvent();
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
                modifiers = null;
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
            OnObjectCreated.Invoke();
        }

        public void AddCreateListener(UnityAction listener){
            OnObjectCreated.AddListener(listener);
        }

        public void AddDestroyListener(UnityAction listener){
            OnObjectDestroyed.AddListener(listener);
        }

        public void AddDataChangeListener(UnityAction listener){
            OnDataChange.AddListener(listener);
        }

        public void AddAttackListener(UnityAction<Entity> listener){
            OnAttack.AddListener(listener);
        }

        public void AddInteractListener(UnityAction<Entity> listener){
            OnInteract.AddListener(listener);
        }

        public void AddReleaseAttackListener(UnityAction<Entity> listener){
            ReleaseAttack.AddListener(listener);
        }

        public void AddReleaseInteractListener(UnityAction<Entity> listener){
            ReleaseInteract.AddListener(listener);
        }

        public void OnAttackEvent(Entity entity){
            OnAttack.Invoke(entity);
        }

        public void OnInteractEvent(Entity entity){
            OnInteract.Invoke(entity);
        }

        public void ReleaseAttackEvent(Entity entity){
            ReleaseAttack.Invoke(entity);
        }

        public void ReleaseInteractEvent(Entity entity){
            ReleaseInteract.Invoke(entity);
        }

        public void DestroyTile(){
            Object.Destroy(gameObject);
        }
    }
}
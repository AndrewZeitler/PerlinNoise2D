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

        public bool CancelItemUse = false;
        public bool objectCreated = false;

        public Vector3 position;

        public Tile(TileData tileData){
            InitializeListeners();
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
            if(tileData == null) {
                return;
            }
            this.tileData = tileData;
            if(objectCreated) {
                foreach(TileModifier modifier in modifiers){
                    modifier.Destroy();
                }
                modifiers = null;
                DestroyTile();
                InitializeListeners();
                InitializeModifiers();
                CreateTileObject(position);
            } else {
                foreach(TileModifier modifier in modifiers){
                    modifier.Destroy();
                }
                modifiers = null;
                InitializeListeners();
                InitializeModifiers();
            }
            OnDataChange.Invoke();
        }

        public void CreateTileObject(Vector2 pos){
            objectCreated = true;
            position = new Vector3(pos.x, pos.y, pos.y);
            if(tileData == TileData.AIR) return;
            gameObject = new GameObject();
            gameObject.transform.localPosition = position;
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            tileScript = gameObject.AddComponent<TileScript>();
            OnObjectCreated.Invoke();
        }

        public void CreateOutline(){
            if(tileScript == null) return;
            tileScript.CreateOutline(spriteRenderer);
        }

        public void DeleteOutline(){
            if(tileScript == null) return;
            tileScript.DeleteOutline();
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

        public void InitializeListeners(){
            OnObjectCreated = new UnityEvent();
            OnObjectDestroyed = new UnityEvent();
            OnDataChange = new UnityEvent();
            OnAttack = new EntityEvent();
            OnInteract = new EntityEvent();
            ReleaseAttack = new EntityEvent();
            ReleaseInteract = new EntityEvent();
        }

        public void DestroyTile(){
            objectCreated = false;
            Object.Destroy(gameObject);
        }
    }
}
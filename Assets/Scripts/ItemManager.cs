using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public struct EditorItem
{
    public string name;
}

[System.Serializable]
public class ItemManager : MonoBehaviour
{
    [HideInInspector]
    public List<EditorItem> editorItems;
    [HideInInspector]
    public List<Item> items;
    private Dictionary<string, Item> nameToItem;

    private void Start() {
        int id = 1;
        items = new List<Item>();
        nameToItem = new Dictionary<string, Item>();

        foreach(EditorItem item in editorItems) {
            items.Add(new Item(id, item.name));
            ++id;
        }
        foreach(Item item in items){
            item.sprite = SpriteManager.GetSprite(item);
            nameToItem.Add(item.name, item);
        }
    }

    public Item GetItem(int id){
        if(id < 0 || id >= items.Count) {return null;}
        return items[id];
    }

    public Item GetItem(string name){
        if(!nameToItem.ContainsKey(name)) return null;
        return nameToItem[name];
    }

    public Sprite GetSprite(int id){
        return items[id].sprite;
    }
}



#if UNITY_EDITOR
 [CustomEditor(typeof(ItemManager))]
 public class ItemInfo_Editor : Editor
 {
    List<bool> showTiles;
    ItemManager manager;

    void OnEnable()
    {
        showTiles = new List<bool>();
        manager = (ItemManager) target;
        if(manager.editorItems == null) manager.editorItems = new List<EditorItem>();
    }

     public override void OnInspectorGUI()
     {
        DrawDefaultInspector();
        
        EditorGUILayout.BeginVertical();
        EditorGUI.indentLevel += 1;

        while(manager.editorItems.Count > showTiles.Count){
            showTiles.Add(false);
        }

        for(int i = 0; i < manager.editorItems.Count; ++i){
            EditorItem script = manager.editorItems[i];
            string name = script.name;
            if(name == null) name = "Item #" + i;
            showTiles[i] = EditorGUILayout.Foldout(showTiles[i], name);

            if(!showTiles[i]) continue;
        
            script.name = EditorGUILayout.TextField("Item Name", script.name);
            manager.editorItems[i] = script;
        }
        EditorGUI.indentLevel -= 1;
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add item")) {
            manager.editorItems.Add(new EditorItem());
            showTiles.Add(false);
        }
        if (GUILayout.Button("Delete item")) {
            manager.editorItems.RemoveAt(manager.editorItems.Count - 1);
            showTiles.RemoveAt(showTiles.Count - 1);
        }
        EditorGUILayout.EndHorizontal();

     }
 }
 #endif

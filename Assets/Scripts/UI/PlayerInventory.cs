using UnityEngine;
using UnityEngine.UI;
using UI.Elements;
using Entities;
using System.Collections;

namespace UI {
    public class PlayerInventoryUI
    {
        static GameObject inventoryPrefab = null;
        Inventory inventory;

        public void CreateUI(Player player, PageUI page){
            this.inventory = player.inventory;

            if(inventoryPrefab == null) inventoryPrefab = Resources.Load("Prefabs/Inventory") as GameObject;
            SceneManager.sceneManager.StartCoroutine(CreateInventory(page));
        }

        public IEnumerator CreateInventory(PageUI page){
            yield return new WaitForEndOfFrame();
            GameObject inventoryUI = GameObject.Instantiate(inventoryPrefab);
            float width = page.viewport.GetComponent<RectTransform>().rect.width;
            inventoryUI.GetComponent<GridLayoutGroup>().cellSize = new Vector2(0.25f * width, 0.25f * width);
            page.SetContent(inventoryUI);
            RectTransform transform = inventoryUI.GetComponent<RectTransform>();
            transform.offsetMax = transform.offsetMin = Vector2.zero;

            for(int i = 0; i < inventory.GetInventorySize(); ++i){
                /////////////////////*******************************//////////////////////////
                OpenSlotUI slot = new OpenSlotUI(i, inventory);
                slot.SetParent(inventoryUI.transform);
            }
        }   
    }
}

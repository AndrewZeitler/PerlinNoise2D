using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements {

    public class HeldItemUI {
        public static GameObject slotPrefab = null;
        public GameObject gameObject { get; private set; }
        public ItemStack itemStack { get; private set; }

        public HeldItemUI(ItemStack itemStack){
            this.itemStack = itemStack;
            if(slotPrefab == null) slotPrefab = Resources.Load("Prefabs/ItemSlot") as GameObject;
            gameObject = GameObject.Instantiate(slotPrefab);
            GameObject.Destroy(gameObject.GetComponent<Image>());
            RectTransform heldTransform = gameObject.GetComponent<RectTransform>();
            heldTransform.anchorMin = heldTransform.anchorMax = new Vector2(0, 1);
            heldTransform.sizeDelta = new Vector2(Screen.height / 10f, Screen.height / 10f);
            gameObject.transform.SetParent(MenuManager.menu.transform.parent);
            gameObject.AddComponent<HeldItem>().SetItemStack(itemStack);
            GameObject.Destroy(gameObject.GetComponent<ItemSlot>());
        }

        public void Destroy(){
            GameObject.Destroy(gameObject);
        }

    }

}
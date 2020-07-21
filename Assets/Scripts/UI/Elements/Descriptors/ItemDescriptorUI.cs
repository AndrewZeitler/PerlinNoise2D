using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements {

    public class ItemDescriptorUI : DescriptorUI {
        static GameObject descriptionPrefab = null;
        public GameObject gameObject { get; private set; }
        public ItemStack itemStack;

        public ItemDescriptorUI(ItemStack itemStack){
            this.itemStack = itemStack;
        }

        public override void CreateUI(){
            if(descriptionPrefab == null) descriptionPrefab = Resources.Load("Prefabs/ItemDescriptor") as GameObject;
            if(itemStack == null) return;
            gameObject = GameObject.Instantiate(descriptionPrefab);
            gameObject.transform.GetChild(0).GetComponent<Text>().fontSize = Screen.height / 32;
            gameObject.transform.GetChild(2).GetComponent<Text>().fontSize = Screen.height / 48;
            gameObject.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height / 3.2f, 0);
            gameObject.transform.SetParent(MenuManager.menu.transform.parent);

            string name = itemStack.GetItem().itemData.Name;
            gameObject.GetComponentInChildren<Text>().text = name;
        }

        public override void DestroyUI(){
            GameObject.Destroy(gameObject);
        }

    }

}
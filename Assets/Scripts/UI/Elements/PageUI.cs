using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.EventSystems;
//using Utils;

namespace UI {

    public class PageUI {

        public static GameObject pagePrefab;
        public static GameObject slotPrefab;
        public GameObject page { get; private set; }
        public GameObject viewport { get; private set; }
        public GameObject content { get; private set; }
        GameObject scroll = null;

        public List<Inventory> inventories = new List<Inventory>();

        public PageUI(string name){
            if(pagePrefab == null) {
                pagePrefab = Resources.Load("Prefabs/Page") as GameObject;
            }
            page = GameObject.Instantiate(pagePrefab);
            page.transform.SetParent(MenuManager.menu.transform);

            GameObject tab = page.transform.GetChild(0).gameObject;
            RectTransform tabTransform = tab.GetComponent<RectTransform>();
            tabTransform.anchorMin = new Vector2(tabTransform.anchorMin.x, 0.9f - (MenuManager.GetPagesCount() / 10f));
            tabTransform.anchorMax = new Vector2(tabTransform.anchorMax.x, 1f - (MenuManager.GetPagesCount() / 10f));

            Button button = tab.GetComponent<Button>();
            button.GetComponentInChildren<Text>().text = name;
            button.onClick.AddListener(delegate { OnTabClick(page); });

            RectTransform pageTransform = page.GetComponent<RectTransform>();
            pageTransform.offsetMin = pageTransform.offsetMax = Vector3.zero;

            GameObject panel = page.transform.GetChild(1).gameObject;

            scroll = panel.transform.GetChild(0).gameObject;
            // RectTransform scrollRect = scroll.AddComponent<RectTransform>();
            // scrollRect.anchorMin = Vector2.zero;
            // scrollRect.anchorMax = Vector2.one;
            // scroll.AddComponent<ScrollRect>();
            // ContentSizeFitter scrollSizeFitter = scroll.AddComponent<ContentSizeFitter>();
            // scrollSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            // scrollSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            // scroll.transform.SetParent(panel.transform);

            viewport = scroll.transform.GetChild(0).gameObject;
            // RectTransform contentTransform = content.AddComponent<RectTransform>();
            // contentTransform.anchorMin = new Vector2(0.025f, 0.5f);
            // contentTransform.anchorMax = new Vector2(0.975f, 0.5f);
            // ContentSizeFitter contentSizeFitter = content.AddComponent<ContentSizeFitter>();
            // contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            // contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            // content.transform.SetParent(scroll.transform);

            // scroll.GetComponent<ScrollRect>().content = contentTransform;
            // scroll.GetComponent<ScrollRect>().scrollSensitivity = 50;
            // RectTransform scrollTransform = scroll.GetComponent<RectTransform>();
            // scrollTransform.offsetMin = scrollTransform.offsetMax = Vector3.zero;
        }

        public void PageChanged(){
            for(int i = 0; i < content.transform.childCount; ++i){
                ItemSlot slot = content.transform.GetChild(i).GetComponent<ItemSlot>();
                if(slot == null) continue;
                ItemStack item = slot.inventory.GetItemStackAt(slot.index);
                Image image = slot.transform.GetChild(0).GetComponent<Image>();
                Text text = slot.transform.GetChild(1).GetComponent<Text>();
                slot.itemStack = item;
                if(item != null){
                    image.sprite = item.GetItem().itemData.ItemSprite;
                    image.color = new Color(1, 1, 1, 1);
                    if(item.GetAmount() > 1){
                        text.text = item.GetAmount().ToString();
                    } else {
                        text.text = "";
                    }
                } else {
                    image.sprite = null;
                    image.color = new Color(1, 1, 1, 0);
                    text.text = "";
                }
            }
        }

        public void OnTabClick(GameObject click){
            MenuManager.OnTabClick(click);
        }

        public void SetContent(GameObject content){
            content.transform.SetParent(viewport.transform);
            scroll.GetComponent<ScrollRect>().content = content.GetComponent<RectTransform>();
            this.content = content;
        }
    }

}
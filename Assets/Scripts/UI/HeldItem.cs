using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Items;

public class HeldItem : MonoBehaviour
{
    public ItemStack itemStack;
    private RectTransform rectTransform;

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector2(Input.mousePosition.x - Screen.width / 2, 
                                                   Input.mousePosition.y - Screen.height / 2);
    }

    public void SetItemStack(ItemStack itemStack){
        this.itemStack = itemStack;
        transform.GetChild(0).GetComponent<Image>().sprite = itemStack.GetItem().itemData.ItemSprite;
        transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        if(itemStack.GetAmount() > 1){
            transform.GetChild(1).GetComponent<Text>().text = itemStack.GetAmount().ToString();
        } else {
            transform.GetChild(1).GetComponent<Text>().text = "";
        }
    }

    void FixedUpdate() {
        // rectTransform.localPosition = new Vector2(Input.mousePosition.x - Screen.width / 2 + rectTransform.sizeDelta.x / 2 + 1, 
        //                                           Input.mousePosition.y - Screen.height / 2 - rectTransform.sizeDelta.y / 2 - 1);
        rectTransform.localPosition = new Vector2(Input.mousePosition.x - Screen.width / 2, 
                                                   Input.mousePosition.y - Screen.height / 2);
    }
}

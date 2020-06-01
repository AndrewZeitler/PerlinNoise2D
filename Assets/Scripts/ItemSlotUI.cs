using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int index;
    public ItemStack itemStack = null;

    public void OnPointerClick(PointerEventData pointer){
        MenuManager.ItemSlotClick(pointer, this.gameObject);
    }

    public void OnPointerEnter(PointerEventData pointer){
        MenuManager.ItemSlotEnter(pointer, this.gameObject);
    }

    public void OnPointerExit(PointerEventData pointer){
        MenuManager.ItemSlotExit(pointer, this.gameObject);
    }
}

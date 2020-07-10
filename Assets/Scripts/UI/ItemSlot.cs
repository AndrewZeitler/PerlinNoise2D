using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UI {
    public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public int index;
        public ItemStack itemStack = null;
        public Inventory inventory = null;

        public UnityAction<PointerEventData, ItemSlot> OnClick;
        public UnityAction<PointerEventData, ItemSlot> OnEnter;
        public UnityAction<PointerEventData, ItemSlot> OnExit;

        public void OnPointerClick(PointerEventData pointer){
            OnClick(pointer, this);
        }

        public void OnPointerEnter(PointerEventData pointer){
            OnEnter(pointer, this);
        }

        public void OnPointerExit(PointerEventData pointer){
            OnExit(pointer, this);
        }
    }
}

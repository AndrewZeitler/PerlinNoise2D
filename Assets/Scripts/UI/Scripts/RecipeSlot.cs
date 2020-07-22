using Crafting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UI.Elements;

namespace UI {
    public class RecipeSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Recipe recipe;
        public RecipeUI recipeUI;

        public UnityAction<PointerEventData, RecipeSlot> OnClick;
        public UnityAction<PointerEventData, RecipeSlot> OnEnter;
        public UnityAction<PointerEventData, RecipeSlot> OnExit;

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

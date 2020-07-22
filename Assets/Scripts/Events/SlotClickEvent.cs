
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UI;

namespace Events
{
    class SlotClickEvent : UnityEvent<PointerEventData, ItemSlot> {}
}
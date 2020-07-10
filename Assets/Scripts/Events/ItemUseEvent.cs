
using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    class ItemUseEvent : UnityEvent<Entities.Entity, Vector3> {}
}
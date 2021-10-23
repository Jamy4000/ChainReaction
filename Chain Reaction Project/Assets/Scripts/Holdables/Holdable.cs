using UnityEngine;

namespace Holdables
{
    public delegate void HoldObject(Holdable holdable);
    public delegate void DropObject(Holdable holdable);

    public class Holdable : MonoBehaviour
    {
        public event HoldObject Held;
        public event DropObject Dropped;

        public void HoldObject()
        {
            OnObjectHold();
        }

        protected virtual void OnObjectHold()
        {
            Held?.Invoke(this);
        }

        public void DropObject()
        {
            OnObjectDropped();
        }

        protected virtual void OnObjectDropped()
        {
            Dropped?.Invoke(this);
        }
    }
}
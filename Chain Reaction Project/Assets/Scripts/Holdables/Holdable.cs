using ChainReaction;
using UnityEngine;

namespace Holdables
{
    public enum HoldableType
    {
        Explosive,
        Crate
    }

    public delegate void HoldObject(Holdable holdable);
    public delegate void DropObject(Holdable holdable);
    public class Holdable : MonoBehaviour
    {
        public bool IsPutDown { get; private set; } = true;
        public event HoldObject Held;
        public event DropObject Dropped;
        public HoldableType Type = HoldableType.Crate;

        public void HoldObject()
        {
            IsPutDown = false;
            OnObjectHold();

        }

        protected virtual void OnObjectHold()
        {
            Held?.Invoke(this);
        }

        public void DropObject()
        {
            IsPutDown = true;
            OnObjectDropped();
        }

        protected virtual void OnObjectDropped()
        {
            Dropped?.Invoke(this);
        }
    }
}

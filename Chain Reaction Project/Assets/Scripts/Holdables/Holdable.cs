using ChainReaction;
using UnityEngine;

namespace Holdables
{
    public delegate void HoldObject(Holdable holdable);
    public delegate void DropObject(Holdable holdable);
    public class Holdable : MonoBehaviour
    {
        public bool IsPutDown { get; private set; } = true;
        public event HoldObject Held;
        public event DropObject Dropped;

        public void HoldObject()
        {
            IsPutDown = false;
            StaticActionProvider.recalculateChainReaction?.Invoke();
            OnObjectHold();

        }

        protected virtual void OnObjectHold()
        {
            Held?.Invoke(this);
        }

        public void DropObject()
        {
            StaticActionProvider.recalculateChainReaction?.Invoke();
            IsPutDown = true;
            OnObjectDropped();
        }

        protected virtual void OnObjectDropped()
        {
            Dropped?.Invoke(this);
        }
    }
}

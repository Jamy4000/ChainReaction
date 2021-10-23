using UnityEngine;

namespace Holdables
{
    public class Holdable : MonoBehaviour
    {
        public bool IsPutDown { get; private set; } = true;

        public void PickUp()  => IsPutDown = false;
        public void PutDown() => IsPutDown = true;
    }
}
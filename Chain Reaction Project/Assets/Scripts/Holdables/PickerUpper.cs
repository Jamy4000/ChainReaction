using ChainReaction;
using UnityEditor;
using UnityEngine;

namespace Holdables
{
    public class PickerUpper : MonoBehaviour
    {
        private Holdable currentHoldable;

        protected virtual Vector3 PutDownPosition => transform.position + Vector3.forward * 2;

        internal void PickHoldableUp(Holdable holdable)
        {
            if (holdable == null || !holdable.IsPutDown)
                return;

            Transform trans = holdable.transform;

            trans.SetParent(transform);
            trans.localPosition = Vector3.zero;

            holdable.HoldObject();
            currentHoldable = holdable;
            
            StaticActionProvider.recalculateChainReaction?.Invoke();
        }

        internal void PutHoldableDown()
        {
            if (currentHoldable == null)
                return;

            Transform trans = currentHoldable.transform;

            trans.SetParent(null);
            trans.position = PutDownPosition;

            currentHoldable.DropObject();
            currentHoldable = null;
            
            StaticActionProvider.recalculateChainReaction?.Invoke();
        }

        internal void KillHoldable()
        {
            if (null != currentHoldable)
            {
                Destroy(currentHoldable);
                currentHoldable = null;
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PickerUpper))]
    public class PickerUpperEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Holdable holdable = FindObjectOfType<Holdable>();

            if (GUILayout.Button("Pick Up"))
                ((PickerUpper)target).PickHoldableUp(holdable);

            if (GUILayout.Button("Put Down"))
                ((PickerUpper)target).PutHoldableDown();

            DrawDefaultInspector();
        }
    }
#endif
}
using ChainReaction;
using UnityEditor;
using UnityEngine;

namespace Holdables
{
    public class PickerUpper : MonoBehaviour
    {
        public Holdable CurrentHoldable { get; private set; }

        protected virtual Vector3 PutDownPosition => transform.position + Vector3.forward * 2;

        internal void PickHoldableUp(Holdable holdable)
        {
            if (holdable == null || !holdable.IsPutDown)
                return;

            Transform trans = holdable.transform;

            trans.SetParent(transform);
            trans.localPosition = Vector3.zero;

            holdable.HoldObject();
            CurrentHoldable = holdable;
            
            StaticActionProvider.RecalculateChainReaction?.Invoke();
        }

        internal void PutHoldableDown()
        {
            if (CurrentHoldable == null)
                return;

            Transform trans = CurrentHoldable.transform;

            trans.SetParent(null);
            trans.position = PutDownPosition;

            CurrentHoldable.DropObject();

            if (CurrentHoldable.Type == HoldableType.Explosive)
                StaticActionProvider.RecalculateChainReaction?.Invoke();

            CurrentHoldable = null;
        }

        internal void KillHoldable()
        {
            if (null != CurrentHoldable)
            {
                Destroy(CurrentHoldable);
                CurrentHoldable = null;
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
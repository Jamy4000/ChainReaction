using UnityEditor;
using UnityEngine;

namespace Holdables
{
    public class PickerUpper : MonoBehaviour
    {
        [SerializeField] private Holdable currentHoldable;

        protected virtual Vector3 PutDownPosition => transform.position + Vector3.forward * 2;

        internal void PickHoldableUp()
        {
            if (currentHoldable == null)
                return;

            Transform trans = currentHoldable.transform;

            trans.SetParent(transform);
            trans.localPosition = Vector3.zero;
        }

        internal void PutHoldableDown()
        {
            if (currentHoldable == null)
                return;

            Transform trans = currentHoldable.transform;

            trans.SetParent(null);
            trans.position = PutDownPosition;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PickerUpper))]
    public class PickerUpperEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Pick Up"))
                ((PickerUpper)target).PickHoldableUp();

            if (GUILayout.Button("Put Down"))
                ((PickerUpper)target).PutHoldableDown();

            DrawDefaultInspector();
        }
    }
#endif
}
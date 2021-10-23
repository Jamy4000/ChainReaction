using Holdables;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    //when drone dies, play electro vfx + sfx

    //killing drones should give points (talk to leonid)

    //TODO: audio
    //explosion
    //drive around
    //random beep
    //(pick up/put down a crate)

    public class Drone : MonoBehaviour
    {
        [Header("References"), SerializeField]
        internal PickerUpper pickerUpper;

        [SerializeField] private NavMeshAgent navMeshAgent;

        [SerializeField] private AudioSource moveSound;

        [SerializeField] private Holdable cratePrefab;

        [Header("Pick Up"), SerializeField, Range(.5f, 10f)]
        private float pickUpRange = 1f;

        public Assignment assignment;

        private void Update()
        {
            if (IsTargetClose())
                assignment.Finish(this);
        }

        public void RefreshInitialState()
        {
            moveSound.pitch = 1 + Random.Range(-.2f, .2f);
            moveSound.Play();

            assignment = null;

            pickerUpper.KillHoldable();
        }

        internal void Kill() => gameObject.SetActive(false); //TODO: play explosion with sound

        internal void GiveAssignment(Assignment newAssignment)
        {
            assignment = newAssignment;
            navMeshAgent.SetDestination(newAssignment.Target);

            if (newAssignment is BringCrateAssignment)
                pickerUpper.PickHoldableUp(Instantiate(cratePrefab));
        }

        private bool IsTargetClose()
        {
            if (assignment is null)
            {
                GoHome();

                return false;
            }

            return Vector3.SqrMagnitude(assignment.Target - transform.position) < pickUpRange * pickUpRange;
        }

        internal void GoHome() => GiveAssignment(new GoHomeAssignment(DroneDispatch.Instance.GetRandomSpawnPoint()));

        internal void KillHoldable() => pickerUpper.KillHoldable();

        #region Assignments

        public abstract class Assignment
        {
            public abstract Vector3 Target { get; }
            public abstract void    Finish(Drone drone);
        }

        public class RetrieveCrateAssignment : Assignment
        {
            public Holdable holdable;
            public RetrieveCrateAssignment(Holdable holdable) { this.holdable = holdable; }

            public override Vector3 Target =>
                holdable != null && holdable.IsPutDown ? holdable.transform.position : Vector3.zero;

            public override void Finish(Drone drone)
            {
                if (holdable != null && holdable.IsPutDown)
                    drone.pickerUpper.PickHoldableUp(holdable);

                drone.GoHome();
            }
        }

        public class BringCrateAssignment : Assignment
        {
            public BringCrateAssignment(Vector3 position) { Target = position; }
            public override Vector3 Target { get; }

            public override void Finish(Drone drone)
            {
                drone.pickerUpper.PutHoldableDown();
                drone.GoHome();
            }
        }

        public class GoHomeAssignment : Assignment
        {
            public GoHomeAssignment(Vector3 position) { Target = position; }
            public override Vector3 Target { get; }

            public override void Finish(Drone drone)
            {
                drone.pickerUpper.KillHoldable();
                drone.gameObject.SetActive(false);
            }
        }

        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Drone))]
    public class DroneEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Kill"))
                ((Drone)target).Kill();

            DrawDefaultInspector();
        }
    }
#endif
}
using System.Collections.Generic;
using Holdables;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
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

        [SerializeField] private AudioSource moveSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioClip   spawnSound;
        [SerializeField] private AudioClip   beepSound;
        [SerializeField] private AudioClip   pickUpSound;

        [SerializeField] private List<Holdable> cratePrefabs = new List<Holdable>();

        [Header("Pick Up"), SerializeField, Range(.5f, 10f)]
        private float pickUpRange = 1f;

        public Assignment assignment;

        private void Start()
        {
            moveSource.Play();
            sfxSource.PlayOneShot(spawnSound);
        }

        private void Update()
        {
            if (IsTargetClose())
                assignment.Finish(this);
        }

        public void RefreshInitialState()
        {
            moveSource.pitch = Random.Range(-1f, 1f);
            moveSource.Play();

            assignment = null;

            pickerUpper.KillHoldable();
        }

        internal void GiveAssignment(Assignment newAssignment)
        {
            assignment = newAssignment;
            navMeshAgent.SetDestination(newAssignment.Target);

            if (newAssignment is BringCrateAssignment)
                pickerUpper.PickHoldableUp(Instantiate(cratePrefabs[Random.Range(0, cratePrefabs.Count)]));
        }

        private bool IsTargetClose()
        {
            if (assignment is null)
            {
                GoHome();

                return false;
            }

            Vector3 distanceVector = assignment.Target - transform.position;
            Vector3 flatDistance   = new Vector3(distanceVector.x, 0f, distanceVector.z);

            return Vector3.SqrMagnitude(flatDistance) < pickUpRange * pickUpRange;
        }

        internal void GoHome() => GiveAssignment(new GoHomeAssignment(DroneDispatch.Instance.GetRandomSpawnPoint()));

        internal void KillHoldable()    => pickerUpper.KillHoldable();
        internal void PlayPickUpSound() => sfxSource.PlayOneShot(pickUpSound);
        internal void PlayBeepUpSound() => sfxSource.PlayOneShot(beepSound);

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
                {
                    drone.pickerUpper.PickHoldableUp(holdable);
                    drone.PlayPickUpSound();
                }

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
                drone.PlayPickUpSound();
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
                GameObject o = drone.gameObject;
                o.SetActive(false);
                Destroy(o);
            }
        }

        #endregion
    }
}
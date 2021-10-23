using System;
using Holdables;
using UnityEditor;
using UnityEngine;

namespace AI
{
    //TODO: drones bring boxes into warehouse
    //some drones spawn with boxes, some without
    //drones take boxes away from warehouse
    //drones move boxes around in warehouse
    //sometimes, drones pick up bombs
    
    //when drone dies, play electro vfx + sfx
    
    //TODO: drone spawn points
    //increase spawn rate over time
    
    //drone AI:
    //spawns with or without crate
    //once has no crate, pick a crate to take up, or leave zone
    
    //killing drones should give points (talk to leonid)
    
    public class Drone : MonoBehaviour
    {
        [Header("References"), SerializeField]
        private PickerUpper pickerUpper;

        [SerializeField] private AudioSource explosionSound;
        
        [Header("Pick Up"), SerializeField, Range(.5f, 10f)]
        private float pickUpRange = 1f;

        private readonly State    currentState = State.Idle;
        private          Holdable approachingHoldable;
        private          Holdable currentHoldable;

        private void Update() => UpdateState();

        private void SetState(State state)
        {
            switch (state)
            {
                case State.Idle:       break;
                case State.Patrolling: break;
                case State.Searching:  break;
                case State.Retrieving: break;
                case State.Bringing:   break;
                case State.Dead:       break;
                default:               throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void UpdateState()
        {
            switch (currentState)
            {
                case State.Idle:       break;
                case State.Patrolling: break;
                case State.Searching:  break;
                case State.Retrieving:
                    if (TryTakeHoldable())
                        SetState(State.Bringing);

                    break;
                case State.Bringing: break;
                case State.Dead:     break;
                default:             throw new ArgumentOutOfRangeException();
            }
        }

        private bool TryTakeHoldable()
        {
            
            bool blub = (approachingHoldable.transform.position - transform.position).sqrMagnitude < pickUpRange * pickUpRange;
            bool blub2 = Vector3.SqrMagnitude(approachingHoldable.transform.position - transform.position) < pickUpRange * pickUpRange;
            
            if (Vector3.Distance(approachingHoldable.transform.position, transform.position) < pickUpRange)
            {
                pickerUpper.PickHoldableUp(approachingHoldable);

                return true;
            }

            return false;
        }

        internal void Kill()
        {
            explosionSound.Play();
        }

        private enum State
        {
            Idle,
            Patrolling,
            Searching,
            Retrieving,
            Bringing,
            Dead
        }
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
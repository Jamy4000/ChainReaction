using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Holdables;
using TeppichsTools.Creation;
using TeppichsTools.Creation.Pools;
using UnityEngine;

namespace AI
{
    public class DroneDispatch : Monoton<DroneDispatch>
    {
        [Header("References"), SerializeField]
        private Drone dronePrefab;

        //TODO: drone spawn points
        //increase spawn rate over time
        [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

        [Header("Spawn Rate"), SerializeField]
        private AnimationCurve curve;

        [SerializeField, Range(0u, 100u)]
        private uint secondsBeforeRampUp = 10u;

        [SerializeField, Range(0u, 100u)]
        private uint rampUpDuration = 90u;

        [SerializeField, Range(0u, 100u)]
        private uint minDrones = 3u;

        [SerializeField, Range(0u, 100u)]
        private uint maxDrones = 15u;

        [SerializeField, Range(0, 100)]
        private float spawnCooldownTime = 3f;

        private PrefabObjectPool<Drone> dronePool;

        private float timeSinceLevelStart;

        private IEnumerable<Holdable> AvailableHoldables =>
            HoldableCollector.collection.Where(holdable => holdable.IsPutDown);

        private void Awake() => dronePool = new PrefabObjectPool<Drone>(dronePrefab, transform);

        private void Start() => StartCoroutine(SpawnRoutine());

        private void Update() => timeSinceLevelStart += Time.deltaTime;

        private IEnumerator SpawnRoutine()
        {
            while (Application.isPlaying)
            {
                if (DoWeNeedMoreDrones())
                    SpawnDrone();

                yield return new WaitForSeconds(spawnCooldownTime);
            }

            bool DoWeNeedMoreDrones()
            {
                int current = dronePool.InUse.Count();

                uint wished = timeSinceLevelStart < secondsBeforeRampUp                  ? minDrones :
                    timeSinceLevelStart           < secondsBeforeRampUp + rampUpDuration ? RampFactor() : maxDrones;

                return current < wished;

                uint RampFactor() => (uint)(minDrones + (maxDrones - minDrones)
                    * curve.Evaluate((rampUpDuration - (timeSinceLevelStart - secondsBeforeRampUp)) / rampUpDuration));
            }
        }

        private void SpawnDrone()
        {
            Drone drone = dronePool.Next();
            drone.transform.position = GetRandomSpawnPoint();

            drone.GiveAssignment(new Drone.BringCrateAssignment(Vector3.forward * 10f));
            //give assignment
            //sometimes give Crate
        }

        public Vector3 GetRandomSpawnPoint()
        {
            if (spawnPoints.Count == 0)
                return Vector3.zero;

            return spawnPoints[Random.Range(0, spawnPoints.Capacity)].position;
        }
    }
}
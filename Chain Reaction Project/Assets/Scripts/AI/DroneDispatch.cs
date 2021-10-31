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

        [SerializeField] private List<Transform> spawnPoints   = new List<Transform>();
        [SerializeField] private List<Transform> dropOffPoints = new List<Transform>();

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

        [SerializeField, Range(0.0f, 1.0f)]
        private float _fetchExplosivesProbability = 0.2f;

        private float timeSinceLevelStart;

        private Coroutine _spawnCoroutine;

        private IEnumerable<Holdable> AvailableCrates =>
            HoldableCollector.collection.Where(holdable => holdable.IsPutDown && holdable.Type == HoldableType.Crate);
        private IEnumerable<Holdable> AvailableExplosives =>
            HoldableCollector.collection.Where(holdable => holdable.IsPutDown && holdable.Type == HoldableType.Explosive);

        private void Start()
        {
            _spawnCoroutine = StartCoroutine(SpawnRoutine());
            SignalBus.GameOver.Listen(StopSpawning);
        }

        private void Update() => timeSinceLevelStart += Time.deltaTime;

        private void OnDestroy()
        {
            StopCoroutine(_spawnCoroutine);
            SignalBus.GameOver.StopListening(StopSpawning);
        }

        private void StopSpawning()
        {
            StopCoroutine(_spawnCoroutine);
        }

        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                if (NeedMoreDrones())
                    SpawnDrone();

                yield return new WaitForSeconds(spawnCooldownTime);
            }

            bool NeedMoreDrones()
            {
                int current = DroneCollecter.collection.Count;

                uint wished = timeSinceLevelStart < secondsBeforeRampUp                  ? minDrones :
                    timeSinceLevelStart           < secondsBeforeRampUp + rampUpDuration ? RampFactor() : maxDrones;

                return current < wished;

                uint RampFactor() => (uint)(minDrones + (maxDrones - minDrones)
                    * curve.Evaluate((rampUpDuration - (timeSinceLevelStart - secondsBeforeRampUp)) / rampUpDuration));
            }
        }

        private void SpawnDrone()
        {
            Drone drone = Instantiate(dronePrefab);
            drone.RefreshInitialState();

            drone.transform.position = GetRandomSpawnPoint();

            Drone.Assignment assignment;
            bool shouldFetchExplosives = Random.value > _fetchExplosivesProbability;
            List<Holdable> availableItems = shouldFetchExplosives ?
                AvailableExplosives.ToList() : AvailableCrates.ToList();

            if (!shouldFetchExplosives && (availableItems.Count == 0 || Random.value < 0.4f))
            {
                assignment =
                    new Drone.BringCrateAssignment(dropOffPoints[Random.Range(0, dropOffPoints.Count - 1)].position);
            }
            else
            {
                assignment = new Drone.RetrieveCrateAssignment(availableItems[Random.Range(0, availableItems.Count - 1)]);
            }

            drone.GiveAssignment(assignment);
        }

        public Vector3 GetRandomSpawnPoint()
        {
            if (spawnPoints.Count == 0)
                return Vector3.zero;

            return spawnPoints[Random.Range(0, spawnPoints.Count - 1)].position;
        }
    }
}
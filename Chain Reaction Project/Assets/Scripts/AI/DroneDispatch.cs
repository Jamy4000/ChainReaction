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
        [SerializeField] private Drone dronePrefab;

        //TODO: drone spawn points
        //increase spawn rate over time
        [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

        private PrefabObjectPool<Drone> dronePool;

        private IEnumerable<Holdable> AvailableHoldables =>
            HoldableCollector.collection.Where(holdable => holdable.IsPutDown);

        private void Awake() => dronePool = new PrefabObjectPool<Drone>(dronePrefab, transform);

        private void SpawnDrone()
        {
            Drone drone = dronePool.Next();
            drone.transform.position = GetRandomSpawnPoint();
        }

        public Vector3 GetRandomSpawnPoint()
        {
            if (spawnPoints.Count == 0)
                return Vector3.zero;

            return spawnPoints[Random.Range(0, spawnPoints.Capacity)].position;
        }
    }
}
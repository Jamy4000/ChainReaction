using AI;
using UnityEngine;

namespace ChainReaction
{

    /// <summary>
    /// Used to destroy crates and drones when player drive on them
    /// </summary>
    public class PlayerObjectDestroyer : MonoBehaviour
    {
        [SerializeField]
        private GameObject _crateDestroyedVFX;
        [SerializeField]
        private AudioSource _crateDestroyedSFX;

        [SerializeField]
        private GameObject _droneDestroyedVFX;
        [SerializeField]
        private AudioSource _droneDestroyedSFX;

        [SerializeField]
        private Forkinteractor _forkInteractor;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(TagsHolder.CRATE_TAG))
            {
                Holdables.Holdable holdable = collision.gameObject.GetComponent<Holdables.Holdable>();
                if (holdable.IsOnFloor)
                    DestroyCrate(holdable);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagsHolder.DRONE_TAG))
            {
                Drone drone = other.GetComponent<Drone>();
                Holdables.Holdable heldObject = drone.pickerUpper.CurrentHoldable;

                if (heldObject != null)
                {
                    switch (heldObject.Type)
                    {
                        case Holdables.HoldableType.Crate:
                            drone.pickerUpper.PutHoldableDown();
                            DestroyCrate(heldObject);
                            break;

                        case Holdables.HoldableType.Explosive:
                            if (_forkInteractor.HeldItem == null)
                                _forkInteractor.PickObject(heldObject);
                            else
                                drone.pickerUpper.PutHoldableDown();
                            break;
                    }
                }

                DestroyDrone(drone.gameObject);
            }
        }

        private void DestroyCrate(Holdables.Holdable crate)
        {
            _forkInteractor.PickableNearFork.Remove(crate);

            if (_crateDestroyedVFX)
                Instantiate(_crateDestroyedVFX, crate.transform.position, Quaternion.identity);

            if (_crateDestroyedSFX)
                _crateDestroyedSFX.Play();

            StaticActionProvider.OnObjectDestroyed?.Invoke(crate.Price);
            Destroy(crate.gameObject);
        }

        private void DestroyDrone(GameObject drone)
        {
            if (_droneDestroyedVFX)
                Instantiate(_droneDestroyedVFX, drone.transform.position + Vector3.up * 0.1f, Quaternion.identity);

            if (_droneDestroyedSFX)
                _droneDestroyedSFX.Play();

            StaticActionProvider.OnObjectDestroyed?.Invoke(Drone.Price);

            Destroy(drone);
        }
    }
}
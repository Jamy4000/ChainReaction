using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Forkinteractor : MonoBehaviour
{
    [SerializeField]
    private AudioSource _itemPickupSound;
    [SerializeField]
    private AudioSource _itemDropSound;

    private HashSet<Collider> _bombNearFork = new HashSet<Collider>();
    private Collider _heldBomb = null;

    [SerializeField]
    private Transform _BombPositionOffset;

    [SerializeField]
    private Transform _dropPositionOffset;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagsHolder.BOMB_TAG))
        {
            _bombNearFork.Add(other);
        }

        //TODO: add drone interaction
        //if player has bomb, player dies
        //else only drone dies

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagsHolder.BOMB_TAG))
        {
            _bombNearFork.Remove(other);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Dropping down crate
            if (_heldBomb != null)
            {
                _heldBomb.transform.SetParent(null);
                _heldBomb.transform.position = _dropPositionOffset.position;
                _heldBomb.GetComponent<Holdables.Holdable>().DropObject();

                _heldBomb = null;

                _itemDropSound.Play();
            }
            else
            {
                // Picking up crate
                if (_bombNearFork.Count > 0)
                {
                    Collider closestCrate = _bombNearFork.First();
                    foreach (Collider crate in _bombNearFork)
                    {
                        if (Vector3.Distance(closestCrate.transform.position, this.transform.position) >
                            Vector3.Distance(crate.transform.position, this.transform.position))
                        {
                            closestCrate = crate;
                        }
                    };
                    closestCrate.transform.SetParent(this.transform);
                    closestCrate.transform.localPosition = _BombPositionOffset.localPosition;
                    closestCrate.transform.localRotation = Quaternion.identity;
                    _heldBomb = closestCrate;
                    _heldBomb.GetComponent<Holdables.Holdable>().HoldObject();

                    _itemPickupSound.Play();
                }
            }
        }
    }


}

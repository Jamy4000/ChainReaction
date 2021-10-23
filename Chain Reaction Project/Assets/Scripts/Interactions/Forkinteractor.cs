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

    private HashSet<Collider> _PickableNearFork = new HashSet<Collider>();
    private Collider _helditem = null;

    [SerializeField]
    private Transform _ItemPositionOffset;

    [SerializeField]
    private Transform _dropPositionOffset;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagsHolder.BOMB_TAG) || other.CompareTag(TagsHolder.CRATE_TAG))
        {
            _PickableNearFork.Add(other);
            Debug.Log("enter: " + _PickableNearFork.Count());
        }

        //TODO: add drone interaction
        //if player has bomb, player dies
        //else only drone dies

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagsHolder.BOMB_TAG) || other.CompareTag(TagsHolder.CRATE_TAG))
        {
            _PickableNearFork.Remove(other);
            Debug.Log("exit: " + _PickableNearFork.Count());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Dropping down crate
            if (_helditem != null)
            {
                _helditem.transform.SetParent(null);
                _helditem.transform.position = _dropPositionOffset.position;
                _helditem.GetComponent<Holdables.Holdable>().DropObject();

                _helditem = null;

                _itemDropSound.Play();
            }
            else
            {
                // Picking up crate
                if (_PickableNearFork.Count > 0)
                {
                    Collider closestCrate = _PickableNearFork.First();
                    foreach (Collider crate in _PickableNearFork)
                    {
                        if (Vector3.Distance(closestCrate.transform.position, this.transform.position) >
                            Vector3.Distance(crate.transform.position, this.transform.position))
                        {
                            closestCrate = crate;
                        }
                    };
                    closestCrate.transform.SetParent(this.transform);
                    closestCrate.transform.localPosition = _ItemPositionOffset.localPosition;
                    closestCrate.transform.localRotation = Quaternion.identity;
                    _helditem = closestCrate;
                    _helditem.GetComponent<Holdables.Holdable>().HoldObject();

                    _itemPickupSound.Play();
                }
            }
        }
    }


}

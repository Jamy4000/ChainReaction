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

    private HashSet<Holdables.Holdable> _PickableNearFork = new HashSet<Holdables.Holdable>();
    private Holdables.Holdable _helditem = null;

    [SerializeField]
    private Transform _ItemPositionOffset;

    [SerializeField]
    private Transform _dropPositionOffset;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagsHolder.BOMB_TAG) || other.CompareTag(TagsHolder.CRATE_TAG))
        {
            Holdables.Holdable holdable = other.GetComponent<Holdables.Holdable>();
            if (holdable) 
            { 
                _PickableNearFork.Add(holdable);
            }
        }

        //TODO: add drone interaction
        //if player has bomb, player dies
        //else only drone dies

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagsHolder.BOMB_TAG) || other.CompareTag(TagsHolder.CRATE_TAG))
        {
            Holdables.Holdable holdable = other.GetComponent<Holdables.Holdable>();
            if (holdable && _PickableNearFork.Contains(holdable))
            {
                _PickableNearFork.Remove(holdable);
            }
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
                    Holdables.Holdable closestCrate = _PickableNearFork.First();
                    Vector3 crateToPlayer = closestCrate.transform.position - transform.position;
                    float bestHorizontalSqDist = crateToPlayer.x * crateToPlayer.x + crateToPlayer.z * crateToPlayer.z;

                    foreach (Holdables.Holdable crate in _PickableNearFork)
                    {
                        // Give priority to explosives
                        if (closestCrate.Type == Holdables.HoldableType.Explosive &&
                            crate.Type == Holdables.HoldableType.Crate)
                            continue;

                        crateToPlayer = crate.transform.position - transform.position;
                        float horizontalSqDist = crateToPlayer.x * crateToPlayer.x + crateToPlayer.z * crateToPlayer.z;
                        if (horizontalSqDist < bestHorizontalSqDist)
                        {
                            closestCrate = crate;
                            bestHorizontalSqDist = horizontalSqDist;
                        }
                    };

                    closestCrate.transform.SetParent(this.transform);
                    closestCrate.transform.localPosition = _ItemPositionOffset.localPosition;
                    closestCrate.transform.localRotation = Quaternion.identity;
                    _helditem = closestCrate;
                    _helditem.HoldObject();

                    _itemPickupSound.Play();
                }
            }
        }
    }


}

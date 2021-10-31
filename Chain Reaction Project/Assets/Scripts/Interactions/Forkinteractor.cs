using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Holdables;
using System;

public class Forkinteractor : MonoBehaviour
{
    [SerializeField]
    private AudioSource _itemPickupSound;
    [SerializeField]
    private AudioSource _itemDropSound;

    public HashSet<Holdables.Holdable> PickableNearFork = new HashSet<Holdables.Holdable>();
    public Holdables.Holdable HeldItem { get; private set; } = null;

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
                PickableNearFork.Add(holdable);
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
            if (holdable && PickableNearFork.Contains(holdable))
            {
                PickableNearFork.Remove(holdable);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Dropping down crate
            if (HeldItem != null)
            {
                HeldItem.transform.SetParent(null);
                HeldItem.transform.position = _dropPositionOffset.position;
                HeldItem.GetComponent<Holdables.Holdable>().DropObject();

                HeldItem = null;

                _itemDropSound.Play();
            }
            else
            {
                // Picking up crate
                if (PickableNearFork.Count > 0)
                {
                    Holdables.Holdable closestCrate = PickableNearFork.First();
                    Vector3 crateToPlayer = closestCrate.transform.position - transform.position;
                    float bestHorizontalSqDist = crateToPlayer.x * crateToPlayer.x + crateToPlayer.z * crateToPlayer.z;

                    foreach (Holdables.Holdable crate in PickableNearFork)
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

                    PickObject(closestCrate);
                }
            }
        }
    }

    public void PickObject(Holdable closestCrate)
    {
        if (!closestCrate.IsPutDown)
            closestCrate.DropObject();

        if (!closestCrate.IsOnFloor)
            closestCrate.IsOnFloor = true;

        closestCrate.transform.SetParent(this.transform);
        closestCrate.transform.localPosition = _ItemPositionOffset.localPosition;
        closestCrate.transform.localRotation = Quaternion.identity;
        HeldItem = closestCrate;

        HeldItem.HoldObject();

        _itemPickupSound.Play();
    }
}

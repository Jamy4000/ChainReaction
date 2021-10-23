using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Forkinteractor : MonoBehaviour
{
    [SerializeField]
    private Vector3 _cratePositionOffset;
    private HashSet<Collider> _crateNearFork = new HashSet<Collider>();

    [SerializeField]
    private Transform _dropPositionOffset;

    private Collider _heldCrate = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagsHolder.CRATE_TAG))
        {
            _crateNearFork.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagsHolder.CRATE_TAG))
        {
            _crateNearFork.Remove(other);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Dropping down crate
            if (_heldCrate != null)
            {
                _heldCrate.transform.SetParent(null);
                _heldCrate.transform.position = _dropPositionOffset.position;
                _heldCrate = null;
            }
            else
            {
                // Picking up crate
                if (_crateNearFork.Count > 0)
                {
                    Collider closestCrate = _crateNearFork.First();
                    foreach (Collider crate in _crateNearFork)
                    {
                        if (Vector3.Distance(closestCrate.transform.position, this.transform.position) >
                            Vector3.Distance(crate.transform.position, this.transform.position))
                        {
                            closestCrate = crate;
                        }
                    };
                    closestCrate.transform.SetParent(this.transform);
                    closestCrate.transform.localPosition = _cratePositionOffset;
                    _heldCrate = closestCrate;
                }
            }
        }
    }
}

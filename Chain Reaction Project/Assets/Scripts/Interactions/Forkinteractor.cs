using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forkinteractor : MonoBehaviour
{
    [SerializeField]
    private Vector3 _cratePositionOffset;
    private List<Collider> _crateNearFork = new List<Collider>();
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
            if (_crateNearFork.Count > 0)
            {
                Collider closestCrate = _crateNearFork[0];
                foreach (Collider crate in _crateNearFork)
                {
                    if (Vector3.Distance(closestCrate.transform.position,this.transform.position)>
                        Vector3.Distance(crate.transform.position,this.transform.position))
                    {
                        closestCrate = crate;
                    } 
                };
                closestCrate.transform.SetParent(this.transform);
                closestCrate.transform.localPosition = _cratePositionOffset;
            }
        }
    }


}

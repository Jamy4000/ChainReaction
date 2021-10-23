using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forkinteractor : MonoBehaviour
{
    [SerializeField]
    private Vector3 _cratePositionOffset;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagsHolder.CRATE_TAG))
        {
            other.transform.SetParent(this.transform);
            other.transform.localPosition = _cratePositionOffset;
        }
    }

    


}

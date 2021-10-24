using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform Target;

    [SerializeField]
    private Vector3 offset;



    void Update()
    {
        transform.position = Target.position + offset;

    }
}

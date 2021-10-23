using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Change the Movement speed of the player")]
    [Range(0f,10f)]
    private float MovemntSpeed;
    [SerializeField]
    [Tooltip("Change the turning speed of the player,the higher the number the faster the turning")]
    [Range(0f, 10f)]
    private float TurningSpeed;
    [SerializeField]
    [Tooltip("Pick the rigid body of the object")]
    private Rigidbody _Rigidbody;

    private float X;
    private float Z;
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        X = Input.GetAxis("Horizontal") * MovemntSpeed;
        Z = Input.GetAxis("Vertical") * MovemntSpeed;

        _Rigidbody.velocity = new Vector3(X,0, Z);

        Vector3 vel = _Rigidbody.velocity;
        //vel.y = 0.0f;
        if (vel.x != 0 || vel.z != 0)
        {
            _Rigidbody.transform.forward = Vector3.Lerp(_Rigidbody.transform.forward, vel, TurningSpeed * Time.deltaTime);
        }
    }
}

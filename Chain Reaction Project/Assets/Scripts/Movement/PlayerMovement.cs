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

    [SerializeField]
    private AudioSource _maxSpeed;
    [SerializeField]
    private AudioSource _idleSound;
    [SerializeField]
    private float _soundWaitingTime = 0.5f;

    private bool _isMoving = false;
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

        _Rigidbody.velocity = new Vector3(X, _Rigidbody.velocity.y, Z);

        Vector3 vel = _Rigidbody.velocity;

        if (vel.x != 0 || vel.z != 0)
        {
            if (!_isMoving)
            {
                if (_idleSound.time > _soundWaitingTime)
                {
                    
                    _idleSound.Stop();
                    _maxSpeed.Play();
                    _isMoving = true;
                }
            }
            _Rigidbody.transform.forward = Vector3.Lerp(_Rigidbody.transform.forward, vel, TurningSpeed * Time.deltaTime);
        }
        else
        {
            
            if (_isMoving)
            {
                if(_maxSpeed.time > _soundWaitingTime)
                {
                    _maxSpeed.Stop();
                    _isMoving = false;
                    _idleSound.Play();
                } 
            }
        }
    }
}

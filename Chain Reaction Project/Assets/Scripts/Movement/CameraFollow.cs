using ChainReaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform Target;
    [SerializeField]
    private Transform MapTopViewPoint;

    [SerializeField]
    private Vector3 offset;

    private bool _followingPlayer = true;
    private bool _hasReachedTopViewPoint = false;

    private void Start()
    {
        SignalBus.GameOver.Listen(MoveCameraOnTop);
    }

    private void OnDestroy()
    {
        SignalBus.GameOver.StopListening(MoveCameraOnTop);
    }

    private void MoveCameraOnTop()
    {
        _followingPlayer = false;
    }

    void Update()
    {
        if (_followingPlayer)
        {
            transform.position = Target.position + offset;
        }
        else
        {
            if (!_hasReachedTopViewPoint)
            {

                _hasReachedTopViewPoint = Vector3.SqrMagnitude(transform.position - MapTopViewPoint.position) < 0.5f;

                if (_hasReachedTopViewPoint)
                    StaticActionProvider.triggerExplosion.Invoke();
            }
        }
    }
}

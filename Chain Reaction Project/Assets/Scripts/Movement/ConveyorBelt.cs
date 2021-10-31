using System;
using System.Collections;
using System.Collections.Generic;
using Holdables;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField]
    private AudioSource _conveySound;
    [SerializeField]
    private AudioSource _spawnSound;
    [SerializeField]
    private AudioSource _dropOffSound;
    [SerializeField]
    private float _speedOfItem;

    [SerializeField]
    private Transform _startPosition;
    [SerializeField]
    private Transform _endPosition;
    [SerializeField]
    private Transform _dropPosition;
    [SerializeField]
    private Transform _midPosition;
    private bool _atEndPoint = false;

    private Holdable _item;
    private float _soundWaitingTime = 0.3f;

    private void Start()
    {
        SignalBus.GameOver.Listen(StopUpdating);
    }

    private void OnDestroy()
    {
        SignalBus.GameOver.StopListening(StopUpdating);
    }

    public void AddItemForBelt(Holdable Item)
    {
        _item = Item;
        _item.transform.SetParent(this.transform);
        _item.Held += Pickup;
        _item.transform.position = _startPosition.position;
        _spawnSound.Play();
    }

    private void Pickup(Holdable holdable)
    {
        _item.Held -= Pickup;
        _item = null;

     //   _atEndPoint = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_item != null)
        {
            if (!_atEndPoint)
            {
                if (_conveySound.time > _soundWaitingTime)
                {

                    _conveySound.Play();
                }
                if (Vector3.Distance(_item.transform.position, _midPosition.position) > 0.05f)
                {
                    _item.transform.position = Vector3.MoveTowards(_item.transform.position, _midPosition.position, _speedOfItem * Time.deltaTime);

                }
                else if (Vector3.Distance(_item.transform.position, _endPosition.position) > 0.05f)
                {
                    _item.transform.position = Vector3.MoveTowards(_item.transform.position, _endPosition.position, _speedOfItem * Time.deltaTime);
                }
                if (Vector3.Distance(_item.transform.position, _endPosition.position) > 0.05f)
                {
                    _atEndPoint = true;
                }

            }

            else
            {
                _conveySound.Stop();
                if (Vector3.Distance(_item.transform.position, _dropPosition.position) > 0.05f)
                {
                    _item.transform.position = Vector3.MoveTowards(_item.transform.position, _dropPosition.position, _speedOfItem * Time.deltaTime);
                }
                else
                {
                    _item.Held -= Pickup;
                    _item = null;
                    _dropOffSound.Play();
                }


            }
        }
    }


    private void StopUpdating()
    {
        this.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField]
    private float _speedOfItem;

    [SerializeField]
    private Transform _startPosition;
    [SerializeField]
    private Transform _endPosition;
    [SerializeField]
    private Transform _dropPosition;
    [SerializeField]
    private bool _atEndPoint = false;

    private Collider _item;

    
    public void AddItemForBelt(Collider Item)
    {
        _item = Item;

        _item.transform.position = _startPosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_item != null)
        {
            if (!_atEndPoint)
            {
                if (Vector3.Distance(_item.transform.position,_endPosition.position)>0.05f)
                {
                    _item.transform.position = Vector3.MoveTowards(_item.transform.position, _endPosition.position, _speedOfItem * Time.deltaTime);
                }
                else
                {
                    _atEndPoint = true;
                }
            }
            else
            {
                _item.transform.position = Vector3.MoveTowards(_item.transform.position, _dropPosition.position, _speedOfItem * Time.deltaTime);

            }
        }
    }
}

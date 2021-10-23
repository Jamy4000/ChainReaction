using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationRandomzier : MonoBehaviour
{
    
    [SerializeField]
    private GameObject _bomb;

    [SerializeField]
    private List<Transform> _locations;


    
    // Start is called before the first frame update
    void Start()
    {
        PlaceStartingBomb();
    }
    
    private void PlaceStartingBomb()
    {
        
        int tmp = Random.Range(1, _locations.Count);
        GameObject _placedBomb = Instantiate(_bomb,_locations[tmp].transform.position,Quaternion.identity);
        _placedBomb.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

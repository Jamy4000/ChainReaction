using System.Collections;
using System.Collections.Generic;
using ChainReaction;
using Holdables;
using UnityEngine;


public class BombRandomizer : MonoBehaviour
{
    [SerializeField]
    private List<Holdable> bombType = new List<Holdable>();

    private float _timeBeforeDrop;   
    private ConveyorBelt _conveyor;

    //public List<Holdable> BombList = new List<Holdable>();


    private bool _toDrop;   // Change functionality to be true after every pick

    // 
    private void Awake()
    {
        _conveyor = GetComponent<ConveyorBelt>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _toDrop = true;
        _timeBeforeDrop = Random.Range(1f, 7f);
    }

    // Update is called once per frame
    void Update()
    {

        if (_toDrop )
        {
            if (_timeBeforeDrop <= 0f)
            {
                _toDrop = false;
                DropBomb();
            }
            else
            {
                _timeBeforeDrop -= Time.deltaTime;
            }
        }
    }

    private void DropBomb()
    {
        int bombtype = Random.Range(0, 50);
        Holdable newBomb = Instantiate(bombtype <= 20 ? bombType[0]:bombType[Random.Range(1,bombType.Count)]) ;
        StaticActionProvider.ExplosivesPlaced?.Invoke();

        // TODO: connect this to the remote bomb
        //BombList.Add(newBomb);

        newBomb.Held += OnBombPick;
        _conveyor.AddItemForBelt(newBomb);

    }

    private void OnBombPick(Holdable bomb)
    {
        bomb.Held -= OnBombPick;
        _timeBeforeDrop = Random.Range(5f, 15f);
        _toDrop = true;
    }
}

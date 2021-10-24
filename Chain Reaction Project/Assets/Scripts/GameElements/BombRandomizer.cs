using System.Collections;
using System.Collections.Generic;
using ChainReaction;
using Holdables;
using UnityEngine;

public class BombRandomizer : MonoBehaviour
{
    [SerializeField]
    private List<Holdable> bombType = new List<Holdable>();

    [SerializeField]
    private List<ConveyorBelt> conveyors;

    public List<Holdable> BombList = new List<Holdable>();


    private bool _toDrop;   // Change functionality to be true after every pick

    // Start is called before the first frame update
    void Start()
    {
        _toDrop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_toDrop)
        {
            _toDrop = false;
            DropBomb();
        }
    }

    private void DropBomb()
    {
        int chosenConveyor = Random.Range(0, conveyors.Count);
        Holdable newBomb = Instantiate(bombType[Random.Range(0, bombType.Count)]);
        StaticActionProvider.explosivesPlaced?.Invoke();

        // TODO: connect this to the remote bomb
        BombList.Add(newBomb);

        newBomb.Held += OnBombPick;
        conveyors[chosenConveyor].AddItemForBelt(newBomb);
    }

    private void OnBombPick(Holdable bomb)
    {
        bomb.Held -= OnBombPick;
        _toDrop = true;
    }
}

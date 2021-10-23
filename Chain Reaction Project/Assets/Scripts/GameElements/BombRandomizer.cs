using System.Collections;
using System.Collections.Generic;
using Holdables;
using UnityEngine;

public class BombRandomizer : MonoBehaviour
{
    [SerializeField]
    private Holdable bomb;

    [SerializeField]
    private List<ConveyorBelt> conveyors;

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
        Debug.Log(chosenConveyor);
        Holdable newBomb = Instantiate(bomb);
        bomb.Held += OnBombPick;
        conveyors[chosenConveyor].AddItemForBelt(newBomb);
    }

    private void OnBombPick(Holdable bomb)
    {
        bomb.Held -= OnBombPick;
        DropBomb();
    }
}

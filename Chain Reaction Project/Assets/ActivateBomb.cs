using System.Collections;
using System.Collections.Generic;
using ChainReaction;
using UnityEngine;

public class ActivateBomb : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            SignalBus.GameOver.Raise();
        }
    }
}

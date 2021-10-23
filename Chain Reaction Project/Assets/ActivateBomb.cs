using System.Collections;
using System.Collections.Generic;
using ChainReaction;
using UnityEngine;

public class ActivateBomb : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        StaticActionProvider.triggerExplosion.Invoke();
        SignalBus.GameOver.Raise();    
    }
}

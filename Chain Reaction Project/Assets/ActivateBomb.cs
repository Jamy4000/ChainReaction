using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBomb : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SignalBus.GameOver.Raise();    
    }
}
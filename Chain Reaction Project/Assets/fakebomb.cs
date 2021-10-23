using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fakebomb : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SignalBus.GameOver.Listen(Explode);
    }

    void Explode()
    {
        Debug.Log("meow");
    }
}

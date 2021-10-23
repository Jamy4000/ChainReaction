using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnderRigidfier : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       // Destroy(this.gameObject.GetComponent(typeof(Rigidbody)) as Rigidbody);
        SignalBus.GameOver.Listen(BecomeRigid);
    }

    void BecomeRigid()
    {
        
        Rigidbody tmp = this.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        tmp.AddForce(new Vector3(200,1000,3000));
        SignalBus.GameOver.StopListening(BecomeRigid);
    }

    private void OnDestroy()
    {
        //SignalBus.GameOver.Reset();
        SignalBus.GameOver.StopListening(BecomeRigid);
    }
}

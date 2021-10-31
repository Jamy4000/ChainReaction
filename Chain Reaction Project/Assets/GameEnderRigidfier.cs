using ChainReaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnderRigidfier : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody tmp = this.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        //StartCoroutine(DestroyDelay());
        SignalBus.GameOver.Listen(BecomeRigid);
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject.GetComponent(typeof(Rigidbody)) as Rigidbody);
    }

    void BecomeRigid()
    {
        
        Rigidbody tmp = this.gameObject.GetComponent<Rigidbody>();
        tmp.isKinematic = false;
        SignalBus.GameOver.StopListening(BecomeRigid);
    }

    private void OnDestroy()
    {
        //SignalBus.GameOver.Reset();
        SignalBus.GameOver.StopListening(BecomeRigid);
    }
}

using System.Collections;
using UnityEngine;

namespace ChainReaction
{
    public class VFXSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject smokeVFXPrefab;

        [SerializeField] private float delay;

        void Start()
        {
            StartCoroutine(SpawnSmokeDelay());
        }

        IEnumerator SpawnSmokeDelay()
        {
            yield return new WaitForSeconds(delay);

            Instantiate(smokeVFXPrefab, transform.position, Quaternion.identity);
        }
    }
}
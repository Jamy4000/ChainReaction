using UnityEngine;
using UnityEngine.VFX;

namespace ChainReaction
{
    public class ExplosionForce : MonoBehaviour
    {
        [SerializeField, Range(.1f, 10f)] private float explosionRadius = 1f;
        [SerializeField, Range(.1f, 10f)] private float explosionForce = 2f;
        [SerializeField] private Transform vfx;

        private void Awake()
        {
            StaticActionProvider.triggerExplosion += Explode;
        }

        private void OnDestroy() => StaticActionProvider.triggerExplosion -= Explode;

        // this will scale the model too :(
        private void OnValidate() => vfx.localScale = Vector3.one * explosionRadius;

        [ContextMenu("Explode")]
        private void Explode()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out Rigidbody rigidbody))
                {
                    Vector3 distance = hitCollider.transform.position - transform.position;

                    float forceMultiplier = explosionRadius - distance.magnitude * 100f;

                    Vector3 direction = distance.normalized;

                    rigidbody.AddForce(direction * explosionForce * forceMultiplier);
                }
            }

            GetComponentInChildren<VisualEffect>().enabled = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
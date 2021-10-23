using System;
using System.Collections;
using UnityEngine;

namespace ChainReaction
{
    public class ExplosionForce : MonoBehaviour
    {
        [SerializeField, Range(.1f, 10f)] private float explosionRadius = 1f;
        [SerializeField, Range(.1f, 10f)] private float explosionForce = 2f;

        [SerializeField] private GameObject explosionVFXPrefab;

        private void Awake() => StaticActionProvider.triggerExplosion += Explode;

        private void OnDestroy() => StaticActionProvider.triggerExplosion -= Explode;

        [ContextMenu("Explode")]
        private void Explode()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

            float totalForce = 0f;

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out Rigidbody rigidbody))
                {
                    Vector3 distance = hitCollider.transform.position - transform.position;

                    float forceMultiplier = explosionRadius - distance.magnitude * 100f;

                    Vector3 direction = distance.normalized;

                    float force = explosionForce * forceMultiplier;

                    rigidbody.AddForce(direction * force);

                    totalForce += force;
                }
            }

            StaticActionProvider.destructionForce?.Invoke(totalForce);

            GameObject vfx = Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity, transform.parent);
            vfx.transform.localScale = Vector3.one * explosionRadius;

            Destroy(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
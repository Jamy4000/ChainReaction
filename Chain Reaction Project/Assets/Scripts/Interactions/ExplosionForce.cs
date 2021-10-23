using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChainReaction
{
    public class ExplosionForce : MonoBehaviour
    {
        [SerializeField] private GameObject model;
        [SerializeField] private GameObject explosionRangeShader;

        [SerializeField, Range(.1f, 10f)] private float explosionRadius = 1f;
        [SerializeField, Range(.1f, 10f)] private float explosionForce = 2f;

        [SerializeField] private GameObject explosionVFXPrefab;

        private void Awake() => StaticActionProvider.triggerExplosion += Explode;

        private void OnDestroy() => StaticActionProvider.triggerExplosion -= Explode;

        private void OnValidate()
        {
            explosionRangeShader.transform.localScale = Vector3.one * explosionRadius * 2;
        }

        [ContextMenu("Explode")]
        private void Explode()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

            float totalForce = 0f;

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out Rigidbody rigidbody))
                {
                    Vector3 distance = transform.position - hitCollider.transform.position;

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

            model.SetActive(false);
            explosionRangeShader.SetActive(false);
        }
    }
}
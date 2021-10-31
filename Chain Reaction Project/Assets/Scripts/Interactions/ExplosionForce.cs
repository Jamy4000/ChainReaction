using UnityEngine;

namespace ChainReaction
{
    public class ExplosionForce : MonoBehaviour
    {
        [SerializeField] private GameObject model;
        public GameObject explosionRangeShader;

        [Range(.1f, 10f)] public float explosionRadius = 1f;
        [SerializeField, Range(.1f, 10f)] private float explosionForce = 2f;
        [SerializeField] private Vector3 explosionOffset = new Vector3(0, 1, 0);

        [SerializeField] private GameObject explosionVFXPrefab;
        [SerializeField] private AudioSource[] explosionSounds;

        private void OnValidate()
        {
            explosionRangeShader.transform.localScale = Vector3.one * explosionRadius * 2;
        }

        [ContextMenu("Explode")]
        public void Explode()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

            float totalForce = 0f;

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out Rigidbody rigidbody))
                {
                    Vector3 distance = transform.position - hitCollider.transform.position;

                    float forceMultiplier = Mathf.Abs(explosionRadius - distance.magnitude) * 100f;

                    Vector3 direction = distance.normalized;

                    float force = explosionForce * forceMultiplier;

                    rigidbody.AddForce(direction * force);

                    totalForce += force;
                }
            }

            StaticActionProvider.DestructionForce?.Invoke(totalForce);

            GameObject vfx = Instantiate(explosionVFXPrefab, transform.position + explosionOffset, Quaternion.identity, transform.parent);
            vfx.transform.localScale = Vector3.one * explosionRadius;

            model.SetActive(false);
            explosionRangeShader.SetActive(false);

            int chosenSound = Random.Range(0, explosionSounds.Length);
            explosionSounds[chosenSound].Play();
        }
    }
}
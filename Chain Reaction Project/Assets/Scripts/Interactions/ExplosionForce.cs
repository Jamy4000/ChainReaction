using UnityEngine;

namespace ChainReaction
{
    public class ExplosionForce : MonoBehaviour
    {
        [SerializeField] private GameObject model;
        public GameObject explosionRangeShader;
        private MeshRenderer _explosionRangeRenderer;

        [field: SerializeField, Range(.1f, 20f)]
        public float ExplosionRadius { get; private set; } = 1f;

        [SerializeField, Range(.1f, 10f)] private float explosionForce = 2f;
        [SerializeField] private Vector3 explosionOffset = new Vector3(0, 1, 0);

        public float ExplosionRadiusSqr { get; private set; } = 1f;

        [SerializeField] private GameObject explosionVFXPrefab;
        [SerializeField] private AudioSource[] explosionSounds;

        public bool HasChainedShader { get; private set; } = false;

        private void OnValidate()
        {
            explosionRangeShader.transform.localScale = Vector3.one * ExplosionRadius * 2f;
        }

        private void Awake()
        {
            _explosionRangeRenderer = explosionRangeShader.GetComponent<MeshRenderer>();
            ExplosionRadiusSqr = ExplosionRadius * ExplosionRadius;
        }

        [ContextMenu("Explode")]
        public void Explode()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, ExplosionRadius);

            float totalForce = 0f;

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out Rigidbody rigidbody))
                {
                    Vector3 distance = hitCollider.transform.position - transform.position ;

                    float forceMultiplier =  Mathf.Abs(ExplosionRadius - distance.magnitude) * 100f;

                    Vector3 direction = distance.normalized;

                    float force = explosionForce * forceMultiplier;

                    rigidbody.AddForce(direction * force);

                    totalForce += force;
                }
            }

            StaticActionProvider.DestructionForce?.Invoke(totalForce);

            GameObject vfx = Instantiate(explosionVFXPrefab, transform.position + explosionOffset, Quaternion.identity);

            model.SetActive(false);
            explosionRangeShader.SetActive(false);

            int chosenSound = Random.Range(0, explosionSounds.Length);
            explosionSounds[chosenSound].Play();
        }

        public void SetChainedColor(Color chainedColor, Color chainedWarningColor)
        {
            if (HasChainedShader)
                return;

            _explosionRangeRenderer.material.SetColor("Color_", chainedColor);
            _explosionRangeRenderer.material.SetColor("WarningSignsColor_", chainedWarningColor);
            HasChainedShader = true;
        }

        public void SetOutOfRangeColor(Color unchainedColor, Color unchainedWarningColor)
        {
            if (!HasChainedShader)
                return;

            _explosionRangeRenderer.material.SetColor("Color_", unchainedColor);
            _explosionRangeRenderer.material.SetColor("WarningSignsColor_", unchainedWarningColor);
            HasChainedShader = false;
        }
    }
}
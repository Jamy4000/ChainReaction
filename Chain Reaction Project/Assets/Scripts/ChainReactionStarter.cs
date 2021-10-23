using System.Collections.Generic;
using UnityEngine;

namespace ChainReaction
{
    [RequireComponent(typeof(ExplosionForce))]
    public class ChainReactionStarter : MonoBehaviour
    {
        ExplosionForce explosionForce;

        public List<ExplosionForce> allExplosives = new List<ExplosionForce>();
        public List<ExplosionForce> chainedExplosives = new List<ExplosionForce>();

        private void Awake()
        {
            explosionForce = GetComponent<ExplosionForce>();
        }

        [ContextMenu("Explode")]
        private void Explode()
        {
            RecalculateChain();

            foreach (var item in chainedExplosives)
                item.Explode();
        }

        private void CalculateChain(ExplosionForce candidate)
        {
            chainedExplosives.Add(candidate);

            List<ExplosionForce> newOrigins = CalculateNewCandidates(candidate, allExplosives);

            foreach (var newCandidate in newOrigins)
                CalculateChain(newCandidate);

            List<ExplosionForce> CalculateNewCandidates(ExplosionForce origin, List<ExplosionForce> candidates)
            {
                List<ExplosionForce> candidatesWithinRange = new List<ExplosionForce>();

                foreach (var explosive in candidates)
                    if (!chainedExplosives.Contains(explosive))
                        if ((explosive.transform.position - origin.transform.position).magnitude < origin.explosionRadius)
                            candidatesWithinRange.Add(explosive);

                return candidatesWithinRange;
            }
        }

        public void RecalculateChain()
        {
            chainedExplosives.Clear();
            CalculateChain(explosionForce);

            // set all to color red

            foreach (var item in chainedExplosives)
                // TODO set the color to green
                item.explosionRangeShader.GetComponent<Material>();
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

namespace ChainReaction
{
    [RequireComponent(typeof(ExplosionForce))]
    public class ChainReactionStarter : MonoBehaviour
    {
        ExplosionForce explosionForce;

        //public List<ExplosionForce> allExplosives = new List<ExplosionForce>();
        public List<ExplosionForce> chainedExplosives = new List<ExplosionForce>();

        [SerializeField, ColorUsageAttribute(true, true)] private Color chainedColor;
        [SerializeField, ColorUsageAttribute(true, true)] private Color chainedWarningColor;
        [SerializeField, ColorUsageAttribute(true, true)] private Color unchainedColor;
        [SerializeField, ColorUsageAttribute(true, true)] private Color unchainedWarningColor;

        private void Awake()
        {
            explosionForce = GetComponent<ExplosionForce>();
            StaticActionProvider.recalculateChainReaction += RecalculateChain;
            StaticActionProvider.triggerExplosion += Explode;


            RecalculateChain();
        }

        private void OnDestroy()
        {
            StaticActionProvider.recalculateChainReaction -= RecalculateChain;
            StaticActionProvider.triggerExplosion -= Explode;
        }

        IEnumerator ExecuteAfterTime(float time,ExplosionForce item, List<ExplosionForce> newOrigins)
        {
            yield return new WaitForSeconds(time);
            foreach (var newCandidate in newOrigins)
            {
                myExplode(newCandidate);
            }
            item.Explode();
            // Code to execute after the delay
        }
        [ContextMenu("Explode")]
        private void Explode()
        {
            chainedExplosives.Clear();
            //RecalculateChain();
            myExplode(explosionForce);
            //foreach (var item in chainedExplosives)
              //  item.Explode();
        }
        
        private void myExplode(ExplosionForce candidate)
        {
            
            List<ExplosionForce> newOrigins = CalculateNewCandidates(candidate, ExplosivesCollector.collection);// allExplosives);
            StartCoroutine(ExecuteAfterTime(0.5f, candidate, newOrigins));
            
            List<ExplosionForce> CalculateNewCandidates(ExplosionForce origin, List<ExplosionForce> candidates)
            {
                List<ExplosionForce> candidatesWithinRange = new List<ExplosionForce>();

                foreach (var explosive in candidates) { 
                    if (!chainedExplosives.Contains(explosive))
                    {
                        if ((explosive.transform.position - origin.transform.position).magnitude < origin.explosionRadius)
                        {
                            candidatesWithinRange.Add(explosive);
                        }
                    }
                }
                return candidatesWithinRange;
            }
        }
        private void CalculateChain(ExplosionForce candidate)
        {
            chainedExplosives.Add(candidate);

            List<ExplosionForce> newOrigins = CalculateNewCandidates(candidate, ExplosivesCollector.collection);// allExplosives);

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

        [ContextMenu("Recalculate")]
        public void RecalculateChain()
        {
            chainedExplosives.Clear();
            CalculateChain(explosionForce);

            foreach (var item in chainedExplosives)
            {
                item.explosionRangeShader.GetComponent<MeshRenderer>().material.SetColor("Color_", chainedColor);
                item.explosionRangeShader.GetComponent<MeshRenderer>().material.SetColor("WarningSignsColor_", chainedWarningColor);

            }

            foreach (var item in ExplosivesCollector.collection.Where(x => !chainedExplosives.Contains(x)))
            {
                item.explosionRangeShader.GetComponent<MeshRenderer>().material.SetColor("Color_", unchainedColor);
                item.explosionRangeShader.GetComponent<MeshRenderer>().material.SetColor("WarningSignsColor_", unchainedWarningColor);
            }
        }
    }
}
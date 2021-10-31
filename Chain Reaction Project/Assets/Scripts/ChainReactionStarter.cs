using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using System;

namespace ChainReaction
{
    [RequireComponent(typeof(ExplosionForce))]
    public class ChainReactionStarter : MonoBehaviour
    {
        ExplosionForce _explosionForce;

        private List<ExplosionForce> _chainedExplosives = new List<ExplosionForce>();
        private List<ExplosionForce> _outOfRangeExplosives = new List<ExplosionForce>();

        [SerializeField, ColorUsageAttribute(true, true)] private Color chainedColor;
        [SerializeField, ColorUsageAttribute(true, true)] private Color chainedWarningColor;
        [SerializeField, ColorUsageAttribute(true, true)] private Color unchainedColor;
        [SerializeField, ColorUsageAttribute(true, true)] private Color unchainedWarningColor;

        private void Awake()
        {
            _explosionForce = GetComponent<ExplosionForce>();
            StaticActionProvider.TriggerExplosion += Explode;
            SignalBus.GameOver.Listen(StopUpdating);
        }

        private void Start()
        {
            RecalculateChain();
        }

        private void Update()
        {
            RecalculateChain();
        }

        private void OnDestroy()
        {
            StaticActionProvider.TriggerExplosion -= Explode;
            SignalBus.GameOver.StopListening(StopUpdating);
        }

        private void StopUpdating()
        {
            this.enabled = false;
        }

        [ContextMenu("Explode")]
        private void Explode()
        {
            _chainedExplosives.Clear();
            ExploseThisBomb(_explosionForce);
        }
        
        private void ExploseThisBomb(ExplosionForce candidate)
        {
            List<ExplosionForce> newOrigins = CalculateNewCandidates(candidate, ExplosivesCollector.collection);// allExplosives);

            foreach (ExplosionForce item in newOrigins)
            {
                ExplosivesCollector.collection.Remove(item);
            }

            StartCoroutine(ExecuteAfterTime(0.5f, candidate, newOrigins));
            
            List<ExplosionForce> CalculateNewCandidates(ExplosionForce origin, List<ExplosionForce> candidates)
            {
                List<ExplosionForce> candidatesWithinRange = new List<ExplosionForce>();

                foreach (var explosive in candidates) 
                { 
                    if (!_chainedExplosives.Contains(explosive))
                    {
                        Vector3 explosiveToOrigin = explosive.transform.position - origin.transform.position;
                        float sqrDistance = Vector3.SqrMagnitude(explosiveToOrigin);

                        if (sqrDistance <= origin.ExplosionRadiusSqr) 
                        {
                            candidatesWithinRange.Add(explosive);
                        }
                    }
                }

                return candidatesWithinRange;
            }
        }

        IEnumerator ExecuteAfterTime(float time, ExplosionForce item, List<ExplosionForce> newOrigins)
        {
            yield return new WaitForSeconds(time);
            foreach (var newCandidate in newOrigins)
            {
                ExploseThisBomb(newCandidate);
            }
            item.Explode();
            // Code to execute after the delay
        }

        private void CalculateChain(ExplosionForce origin, List<ExplosionForce> explosivesWithinRange, List<ExplosionForce> outOfRangeExplosives)
        {
            explosivesWithinRange.Add(origin);
            outOfRangeExplosives.Remove(origin);

            List<ExplosionForce> chainedExplosives = CalculateChainedExplosives(origin, outOfRangeExplosives);// allExplosives);

            foreach (ExplosionForce explosive in chainedExplosives)
                CalculateChain(explosive, explosivesWithinRange, outOfRangeExplosives);


            List<ExplosionForce> CalculateChainedExplosives(ExplosionForce origin, List<ExplosionForce> candidates)
            {
                List<ExplosionForce> candidatesWithinRange = new List<ExplosionForce>();

                foreach (ExplosionForce explosive in candidates)
                {
                    if (explosivesWithinRange.Contains(explosive))
                        continue;

                    Vector3 explosiveToOrigin = explosive.transform.position - origin.transform.position;
                    float sqrDistance = Vector3.SqrMagnitude(explosiveToOrigin);

                    if (sqrDistance <= origin.ExplosionRadiusSqr)
                    {
                        candidatesWithinRange.Add(explosive);
                    }
                }

                return candidatesWithinRange;
            }
        }

        [ContextMenu("Recalculate")]
        public void RecalculateChain()
        {
            _chainedExplosives.Clear();
            _outOfRangeExplosives.Clear();
            _outOfRangeExplosives.AddRange(ExplosivesCollector.collection);

            CalculateChain(_explosionForce, _chainedExplosives, _outOfRangeExplosives);

            foreach (ExplosionForce item in _chainedExplosives)
            {
                item.SetChainedColor(chainedColor, chainedWarningColor);
            }

            foreach (ExplosionForce item in _outOfRangeExplosives)
            {
                item.SetOutOfRangeColor(unchainedColor, unchainedWarningColor);
            }
        }
    }
}
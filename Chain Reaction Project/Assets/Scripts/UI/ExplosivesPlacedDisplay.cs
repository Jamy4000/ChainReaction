using TMPro;
using UnityEngine;
using DG.Tweening;

namespace ChainReaction.UI
{
    public class ExplosivesPlacedDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currentCounter;
        //[SerializeField] private TextMeshProUGUI maxCounter;

        //private int max = 10;
        private float current = 0;

        private void Awake()
        {
            StaticActionProvider.OnObjectDestroyed += UpdateText;
            //StaticActionProvider.setExplosivesCount += SetExplosivesCount;
        }

        private void OnDestroy()
        {
            StaticActionProvider.OnObjectDestroyed -= UpdateText;
            //StaticActionProvider.setExplosivesCount -= SetExplosivesCount;
        }

        //private void SetExplosivesCount(int count)
        //{
        //    max = count;
        //    maxCounter.text = $"/{max}";
        //}

        private void UpdateText(float priceToAdd)
        {
            currentCounter.text = $"{(current + priceToAdd).ToString()} $";
        }
    }
}
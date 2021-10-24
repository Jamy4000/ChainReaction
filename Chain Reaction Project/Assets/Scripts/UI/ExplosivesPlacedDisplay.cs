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
        private int current = 0;

        private void Awake()
        {
            StaticActionProvider.explosivesPlaced += UpdateText;
            //StaticActionProvider.setExplosivesCount += SetExplosivesCount;
        }

        private void OnEnable() => UpdateText();

        private void OnDestroy()
        {
            StaticActionProvider.explosivesPlaced -= UpdateText;
            //StaticActionProvider.setExplosivesCount -= SetExplosivesCount;
        }

        //private void SetExplosivesCount(int count)
        //{
        //    max = count;
        //    maxCounter.text = $"/{max}";
        //}

        private void UpdateText() => currentCounter.text = (current + 1).ToString(); //currentCounter..DOCounter(current, newValue, .2f); // why is DOTween not working??
    }
}
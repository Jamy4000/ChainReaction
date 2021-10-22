using TMPro;
using UnityEngine;

namespace ChainReaction.UI
{
    public class ExplosivesPlacedDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI explosivesCounter;

        [SerializeField] private string explosivesText;

        private int max = 10;

        private void Awake()
        {
            StaticActionProvider.explosivesPlaced += UpdateText;
            StaticActionProvider.setExplosivesCount += SetExplosivesCount;
        }

        private void OnEnable() => UpdateText(0);

        private void OnDestroy()
        {
            StaticActionProvider.explosivesPlaced -= UpdateText;
            StaticActionProvider.setExplosivesCount -= SetExplosivesCount;
        }

        private void SetExplosivesCount(int count) => max = count;

        private void UpdateText(int current) => explosivesCounter.text = $"{explosivesText}: {current}/{max}";
    }
}
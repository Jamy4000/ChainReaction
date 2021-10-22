using TMPro;
using UnityEngine;

namespace ChainReaction.UI
{
    public class ExplosivesPlaced : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI explosivesCounter;

        [SerializeField] private string explosivesText;

        //private void Awake() => SomeSingleton.SomeSignal += UpdateText;
        //private void OnDestroy() => SomeSingleton.SomeSignal -= UpdateText;

        private void UpdateText(int current, int max) => explosivesCounter.text = $"{explosivesText}: {current}/{max}";
    }
}
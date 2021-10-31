using ChainReaction;
using TMPro;
using UnityEngine;

public class EndScreenDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private GameObject endScreen;

    private float force = 0;

    private void Awake()
    {
        StaticActionProvider.TriggerExplosion += ShowEndScreen;
        StaticActionProvider.DestructionForce += UpdateCounter;
    }

    private void OnDestroy()
    {
        StaticActionProvider.TriggerExplosion -= ShowEndScreen;
        StaticActionProvider.DestructionForce -= UpdateCounter;
    }

    void ShowEndScreen()
    {
        endScreen.SetActive(true);
    }

    void UpdateCounter(float addedForce)
    {
        force += addedForce;
        counterText.text = force.ToString();
    }
}

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
        StaticActionProvider.triggerExplosion += ShowEndScreen;
        StaticActionProvider.destructionForce += UpdateCounter;
    }

    private void OnDestroy()
    {
        StaticActionProvider.triggerExplosion -= ShowEndScreen;
        StaticActionProvider.destructionForce -= UpdateCounter;
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

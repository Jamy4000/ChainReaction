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
        StartCoroutine(ShowAfterDelay());

        System.Collections.IEnumerator ShowAfterDelay()
        {
            yield return new WaitForSeconds(5.0f);
            endScreen.SetActive(true);
        }
    }

    void UpdateCounter(float addedForce)
    {
        force += (addedForce * 0.01f);
        counterText.text = $"{force.ToString()} $";
    }
}

using ChainReaction;
using TMPro;
using UnityEngine;

public class EndScreenDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private GameObject endScreen;

    private float finalDamages = 0f;

    private void Awake()
    {
        finalDamages = 0f;
        StaticActionProvider.AllExplosionsDone += ShowEndScreen;
        StaticActionProvider.DestructionForce += UpdateCounter;
        StaticActionProvider.OnObjectDestroyed += UpdateCounter;
    }

    private void OnDestroy()
    {
        StaticActionProvider.AllExplosionsDone -= ShowEndScreen;
        StaticActionProvider.DestructionForce -= UpdateCounter;
        StaticActionProvider.OnObjectDestroyed += UpdateCounter;
    }

    void ShowEndScreen()
    {
        StartCoroutine(ShowAfterDelay());

        System.Collections.IEnumerator ShowAfterDelay()
        {
            yield return new WaitForSeconds(2.0f);
            endScreen.SetActive(true);
        }
    }

    void UpdateCounter(float addedDamages)
    {
        finalDamages += (addedDamages * 0.01f);
        counterText.text = $"{finalDamages.ToString()} $";
    }
}

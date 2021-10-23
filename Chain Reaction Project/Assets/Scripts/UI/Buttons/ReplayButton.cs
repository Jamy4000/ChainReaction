using UnityEngine.SceneManagement;

namespace ChainReaction.UI
{
    public class ReplayButton : AddListenerButton
    {
        protected override void OnClick() => SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
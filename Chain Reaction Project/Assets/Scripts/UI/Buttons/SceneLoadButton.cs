using ChainReaction.AttributeRef;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChainReaction.UI
{
    public class SceneLoadButton : AddListenerButton
    {
        [SceneRef, SerializeField]
        private string sceneToLoad;

        protected override void OnClick() => SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
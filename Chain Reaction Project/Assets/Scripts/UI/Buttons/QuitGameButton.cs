using UnityEngine;

namespace ChainReaction.UI
{
    public class QuitGameButton : AddListenerButton
    {
        protected override void OnClick() => Application.Quit();
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace ChainReaction.UI
{
    /// <summary>
    /// This class provides a virtual funtion that is automatically assigned to the buttons 'OnClick' event
    /// </summary>
    [RequireComponent(typeof(Button))]
    public abstract class AddListenerButton : MonoBehaviour
    {
        protected Button button;

        void Awake()
        {
            button = GetComponent<Button>();

            button?.onClick.AddListener(OnClick);
        }

        protected abstract void OnClick();
    }
}
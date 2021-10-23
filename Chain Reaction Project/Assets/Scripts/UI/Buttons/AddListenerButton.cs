using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;

namespace ChainReaction.UI
{
    /// <summary>
    /// This class provides a virtual funtion that is automatically assigned to the buttons 'OnClick' event
    /// </summary>
    [RequireComponent(typeof(Button))]
    public abstract class AddListenerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        protected Button button;

        [SerializeField] private string buttonText;

        private bool isHovering = false;
        [SerializeField] private float animationDuration = 1f;

        [Space]
        [SerializeField] private ButtonColors colors;

        private Sequence scaleUpAnimation;
        private Sequence scaleDownAnimation;

        void OnValidate() => GetComponentInChildren<TextMeshProUGUI>().text = buttonText;

        void Awake()
        {
            button = GetComponent<Button>();

            button?.onClick.AddListener(OnClick);
        }

        protected abstract void OnClick();

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovering = true;
            ScaleUpAnimation(animationDuration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovering = false;
            scaleUpAnimation.Pause();
            scaleDownAnimation.Pause();
            ScaleDownAnimation(.2f);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isHovering = false;

            scaleUpAnimation.Pause();
            scaleDownAnimation.Pause();

            Sequence downAnimation = DOTween.Sequence()
                 .Append(button.transform.DOScale(.98f, .1f).SetEase(Ease.InOutSine))
                 .Join(button.targetGraphic.DOColor(colors.colorBlock.pressedColor, .1f).SetEase(Ease.InOutSine));
        }

        public void OnPointerUp(PointerEventData eventData) => button.transform.DOScale(1f, .1f).SetEase(Ease.InOutSine);

        private void ScaleUpAnimation(float duration)
        {
            float from = 1f;
            float to = 1.05f;
            duration *= Mathf.Abs(to - transform.localScale.z) / Mathf.Abs(to - from);

            scaleUpAnimation = DOTween.Sequence()
                .Append(button.transform.DOScale(to, duration).SetEase(Ease.InOutSine))
                .Join(button.targetGraphic.DOColor(colors.colorBlock.highlightedColor, duration).SetEase(Ease.InOutSine))
                .OnComplete(() => { ScaleDownAnimation(animationDuration); })
                .Pause()
                ;

            if (isHovering)
                scaleUpAnimation.Restart();
        }

        private void ScaleDownAnimation(float duration)
        {
            float from = 1.05f;
            float to = 1f;
            duration *= Mathf.Abs(to - transform.localScale.z) / Mathf.Abs(to - from);

            scaleDownAnimation = DOTween.Sequence()
                .Append(button.transform.DOScale(to, duration).SetEase(Ease.InOutSine))
                .Join(button.targetGraphic.DOColor(colors.colorBlock.normalColor, duration).SetEase(Ease.InOutSine))
                .OnComplete(() => { ScaleUpAnimation(animationDuration); })
                ;
        }
    }
}
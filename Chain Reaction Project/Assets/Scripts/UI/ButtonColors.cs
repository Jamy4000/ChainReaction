using UnityEngine;
using UnityEngine.UI;

namespace ChainReaction
{
    [CreateAssetMenu(fileName = "new ButtonColors", menuName = "ScriptableObjects/UI/ButtonColor")]
    public class ButtonColors : ScriptableObject
    {
        public ColorBlock colorBlock = new ColorBlock();

        private void Awake()
        {
            colorBlock.normalColor = Color.white;
            colorBlock.highlightedColor = Color.white;
            colorBlock.pressedColor = Color.white;
            colorBlock.selectedColor = Color.white;
            colorBlock.disabledColor = Color.white;

            colorBlock.colorMultiplier = 1;
        }
    }
}
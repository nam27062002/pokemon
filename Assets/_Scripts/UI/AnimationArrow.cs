using _Scripts.ObjectSO;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class AnimationArrow : MonoBehaviour
    {
        [SerializeField] private float duration = 1f;
        [SerializeField] private Sprites arrows;
        [SerializeField] private Image image;
        private int index = 0;
        private float timer = 0;
        private void Update()
        {
            if((timer+=Time.deltaTime) >= (duration / arrows.spritesList.Count))
            {
                timer = 0;
                image.sprite = arrows.spritesList[index];
                index = (index + 1) % arrows.spritesList.Count;
            }
        }
    }
}

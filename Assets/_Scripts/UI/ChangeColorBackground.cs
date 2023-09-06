using System;
using UnityEngine;

namespace _Scripts.UI
{
    public class ChangeColorBackground : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color colorDefault;
        [SerializeField] private Color colorHover;

        private void Awake()
        {
            MouseExit();
        }

        public void MouseHover()
        {
            SetColorBackground(colorHover);
        }
        public void MouseExit()
        {
            SetColorBackground(colorDefault);
        }
        private void SetColorBackground(Color color)
        {
            spriteRenderer.color = color;
        }
    }
}

using System;
using UnityEngine;

namespace _Scripts.Pikachu
{
    public class PikachuSingleManager : MonoBehaviour
    {
        [SerializeField] private GameObject pikachuSelected;
        [SerializeField] private SpriteRenderer rendererPikachu;
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private Color colorDefault;
        [SerializeField] private Color colorHover;
        private bool isSelected;

        private void Start()
        {
            isSelected = false;
        }

        public void SetSpritePikachu(Sprite sprite)
        {
            rendererPikachu.sprite = sprite;
        }

        private void OnMouseOver()
        {
            background.color = colorHover;
        }

        private void OnMouseExit()
        {
            background.color = colorDefault;
        }

        private void OnMouseDown()
        {
            isSelected = !isSelected;
            pikachuSelected.SetActive(isSelected);
        }
    }
}

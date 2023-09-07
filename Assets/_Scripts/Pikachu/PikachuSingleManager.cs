using System.Drawing;
using TMPro;
using UnityEngine;
using Color = UnityEngine.Color;

namespace _Scripts.Pikachu
{
    public class PikachuSingleManager : MonoBehaviour
    {
        [SerializeField] private GameObject pikachuSelected;
        [SerializeField] private SpriteRenderer rendererPikachu;
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private Color colorDefault;
        [SerializeField] private Color colorHover;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        private Point point;
        private bool isActive;
        public Point Point => point;
        private bool isSelected;

        private void Start()
        {
            isActive = true;
            isSelected = false;
        }

        public void SetPoint(Point p)
        {
            point = p;
        }
        public void SetSpritePikachu(Sprite sprite)
        {
            rendererPikachu.sprite = sprite;
        }

        public void SetText(string text)
        {
            textMeshProUGUI.text = text;
        }
        private void OnMouseOver()
        {
            if (!isActive) return;
            background.color = colorHover;
        }

        private void OnMouseExit()
        {
            if (!isActive) return;
            background.color = colorDefault;
        }

        private void OnMouseDown()
        {
            if (!isActive) return;
            SetActiveSelected();
        }

        public void SetActiveSelected()
        {
            isSelected = !isSelected;
            pikachuSelected.SetActive(isSelected);
            GameManager.Instance.SetPointClicked(this);
            
        }
        
        public void Hide()
        {
            isActive = false;
            gameObject.SetActive(false);
        }
    }
}

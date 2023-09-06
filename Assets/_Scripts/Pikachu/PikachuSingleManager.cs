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
        public Point Point => point;
        private bool isSelected;

        private void Start()
        {
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
            background.color = colorHover;
        }

        private void OnMouseExit()
        {
            background.color = colorDefault;
        }

        private void OnMouseDown()
        {
            SetActiveSelected();
        }

        public void SetActiveSelected()
        {
            isSelected = !isSelected;
            pikachuSelected.SetActive(isSelected);
            if (isSelected)
            {
                GameManager.Instance.SetPointClicked(this);
            }
        }
        
        public void OnDestroy()
        {
            Destroy(gameObject);
        }
    }
}

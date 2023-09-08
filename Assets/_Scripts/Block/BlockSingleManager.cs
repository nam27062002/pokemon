using System;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

namespace _Scripts.Block
{
    public class BlockSingleManager : MonoBehaviour
    {
        [SerializeField] private GameObject blockSelected;
        [SerializeField] private GameObject modelSprite;
        [SerializeField] private SpriteRenderer background;
        [SerializeField] [Range(0,1)] private float borderWidthPercentage = 0.2f;
        [SerializeField] [Range(0, 1)] private float offset = 0.1f;
        [SerializeField] private Color defaultColor = Color.green;
        [SerializeField] private Color hoverColor = Color.red;
        [SerializeField] private BlockAnimator blockAnimator;
        private Point pointBlock;
        public Point PointBlock => pointBlock;
        private bool isSelected = false;
        
        private void Start()
        {
            SetSizeBackground();
        }

        private void SetSizeBackground()
        {
            background.transform.localScale = new Vector3(1 - offset, 1 - offset,1);
            blockSelected.transform.localScale = new Vector3(1 - offset, 1 - offset,1);
        }
        
        private void SetSizeObjectModelSprite(Sprite sprite)
        {
            Vector3 spriteSize = sprite.bounds.size;
            modelSprite.transform.localScale = new Vector3(1/spriteSize.x * (1 - borderWidthPercentage),1/spriteSize.y * (1 - borderWidthPercentage),1);
        }

        
        public void SetBlock(Sprite sprite,string nameBlock,int i,int j)
        {
            pointBlock = new Point(j, i);
            transform.name = nameBlock;
            if (sprite == null)
            {
                gameObject.SetActive(false);
            }
            else
            {
                SetSpriteBlock(sprite);
                SetSizeObjectModelSprite(sprite); 
            }

        }

        public void SetSpriteBlock(Sprite sprite)
        {
            if (gameObject.activeSelf) modelSprite.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        private void SetBackgroundColor(Color color)
        {
            background.GetComponent<SpriteRenderer>().color = color;
        }
        
        private void OnMouseExit()
        {
            SetBackgroundColor(defaultColor);
        }

        private void OnMouseOver()
        {
            SetBackgroundColor(hoverColor);
        }

        private void OnMouseDown()
        {
            isSelected = !isSelected;
            blockSelected.SetActive(isSelected);
            GameManager.Instance.ClickedBlockHandle(this);
        }

        public void UnSelect()
        {
            blockSelected.SetActive(false);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetBlockSuggest(bool active)
        {
            blockAnimator.SetAnimator(active);
        }
        
    }
}

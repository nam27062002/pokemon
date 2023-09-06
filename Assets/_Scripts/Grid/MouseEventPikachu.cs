using System;
using _Scripts.UI;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Grid
{
    public class MouseEventPikachu : MonoBehaviour
    {
        private int x;
        private int y;
        private ChangeColorBackground changeColorBackground;
        public void SetPoint(int x, int y,ChangeColorBackground changeColorBackground)
        {
            this.x = x;
            this.y = y;
            this.changeColorBackground = changeColorBackground;
        }

        private void OnMouseDown()
        {
            Debug.Log(ToString());
        }

        private void OnMouseOver()
        {
            changeColorBackground.MouseHover();
        }

        private void OnMouseExit()
        {
            changeColorBackground.MouseExit();
        }

        public override string ToString()
        {
            return $"Clicked {y} - {x}";
        }
    }
}

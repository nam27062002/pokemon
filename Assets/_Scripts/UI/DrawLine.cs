using System.Collections.Generic;
using _Scripts.Singleton;
using UnityEngine;

namespace _Scripts.UI
{
    public class DrawLine : Singleton<DrawLine>
    {
        private LineRenderer lineRenderer;
        private void Start()
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            SetLineArgument();
        }

        private void SetLineArgument()
        {
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
        }
        public void SetLines(List<Vector3> points)
        {
            lineRenderer.positionCount = points.Count;
            
            for (int i = 0; i < points.Count; i++)
            {
                lineRenderer.SetPosition(i, points[i]);
            }
            Show();
            Invoke("Hide", .2f);
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }
        private void Hide()
        {
            gameObject.SetActive(false);
        }
        
    }
}
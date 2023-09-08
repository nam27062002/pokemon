using _Scripts.Singleton;
using UnityEngine;

namespace _Scripts.Camera
{
    public class CameraManager : Singleton<CameraManager> 
    {
        [SerializeField] private float outlineCamera = .2f;
        private UnityEngine.Camera mainCamera;
        
        private void Awake()
        {
            mainCamera = GetComponent<UnityEngine.Camera>();
        }
        public void ResizeCameraToFitGrid(float width,float height)
        {
            if (mainCamera == null) return;
            mainCamera.orthographicSize = ((width > height * mainCamera.aspect) ? width / mainCamera.pixelWidth * mainCamera.pixelHeight : height) / 2 + outlineCamera;
        }
        
    }
}

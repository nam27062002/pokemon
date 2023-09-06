using System;
using _Scripts.Grid;
using UnityEngine;

namespace _Scripts.Camera
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private SpawnGrid spawnGrid;
        [SerializeField] private float outlineCamera = .2f;
        private UnityEngine.Camera mainCamera;
        
        private void Awake()
        {
            mainCamera = GetComponent<UnityEngine.Camera>();
        }

        private void Start()
        {
            spawnGrid.OnFinishedSpawnGrid += SpawnGridOnOnFinishedSpawnGrid;
        }

        private void SpawnGridOnOnFinishedSpawnGrid(object sender, SpawnGrid.FinishedSpawnEventArgs e)
        {
            ResizeCameraToFitGrid(e.GridWidth, e.GridHeight, e.GridSize);
        }

        private void ResizeCameraToFitGrid(float width,float height,Vector3 gridSize)
        {
            if (mainCamera != null)
            {
                float cameraWidth = width * gridSize.x;
                float cameraHeight = height * gridSize.y;
                mainCamera.orthographicSize = ((cameraWidth > cameraHeight * mainCamera.aspect) ? cameraWidth / mainCamera.pixelWidth * mainCamera.pixelHeight : cameraHeight) / 2 + outlineCamera;
            }
        }
    }
}

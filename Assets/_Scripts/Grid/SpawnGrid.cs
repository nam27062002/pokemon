using System;
using System.Collections.Generic;
using System.Drawing;
using _Scripts.ObjectSO;
using _Scripts.Pikachu;
using _Scripts.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Grid
{
    public class SpawnGrid : MonoBehaviour
    {
        [SerializeField] private int width = 18;
        [SerializeField] private int height = 11;
        [SerializeField] private Sprites sprites;
        [SerializeField] private float offset = 0.05f;
        [SerializeField] private GameObject pikachuPrefab;
        [SerializeField] private GameObject pikachuPrefabTest;
        
        private Vector3 gridSize;
        private int[,] matrix;
        private List<PikachuSingleManager> pikachuSingleManagers;
        public event EventHandler<FinishedSpawnEventArgs> OnFinishedSpawnGrid;
        public class FinishedSpawnEventArgs : EventArgs
        {
            public int GridWidth;
            public int GridHeight;
            public Vector3 GridSize;
            public int[,] Matrix;
            public List<PikachuSingleManager> PikachuSingleManagers;
        }

        private void Start()
        {
            matrix = new int[height,width];
            pikachuSingleManagers = new List<PikachuSingleManager>();
            SetGridSize();
            SpawnRandomGrid();
            
            OnFinishedSpawnGrid?.Invoke(this, new FinishedSpawnEventArgs()
            {
                GridWidth = width,
                GridHeight = height,
                GridSize = gridSize,
                Matrix = matrix,
                PikachuSingleManagers = pikachuSingleManagers,
            });
        }
        

        private void SetGridSize()
        {
            if (sprites != null && sprites.spritesList.Count != 0)
            {
                Sprite firstSprite = sprites.spritesList[0];
                gridSize = firstSprite.bounds.size;
                gridSize.x += offset;
                gridSize.y += offset;
            }
            else
            {
                gridSize = Vector3.one; 
            }
        }

        private void SpawnRandomGrid()
        {
            Vector3 startPosition = new Vector3(-(width - 1) * gridSize.x / 2, (height - 1) * gridSize.y / 2, 0);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0 || i == height - 1 || j == 0 || j == width - 1)
                    {
                        matrix[i, j] = -1;
                        GameObject pikachu = Instantiate(pikachuPrefabTest);
                        pikachu.transform.position = startPosition + new Vector3(j * gridSize.x, -i * gridSize.y, 0);
                        pikachu.transform.parent = transform;
                    }
                    else
                    {
                        int randomNumber = Random.Range(0, sprites.spritesList.Count);
                        matrix[i, j] = randomNumber;
                        Sprite selectedSprite = sprites.spritesList[randomNumber];
                        GameObject pikachu = Instantiate(pikachuPrefab);
                        PikachuSingleManager pikachuSingleManager = pikachu.GetComponent<PikachuSingleManager>();
                        pikachuSingleManager.SetSpritePikachu(selectedSprite);
                        pikachuSingleManager.SetText(randomNumber.ToString());
                        pikachuSingleManager.SetPoint(new Point(j,i));
                        pikachuSingleManagers.Add(pikachuSingleManager);
                        pikachu.transform.position = startPosition + new Vector3(j * gridSize.x, -i * gridSize.y, 0);
                        pikachu.transform.parent = transform;
                    }

                }
            }
        }
        
    }
}

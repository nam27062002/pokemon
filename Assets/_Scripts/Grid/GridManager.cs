using System.Collections.Generic;
using System.Drawing;
using _Scripts.Block;
using _Scripts.Camera;
using _Scripts.ObjectSO;
using _Scripts.Singleton;
using UnityEngine;

namespace _Scripts.Grid
{
    public class GridManager : Singleton<GridManager>
    {
        [SerializeField] private int width = 18;
        [SerializeField] private int height = 11;
        [SerializeField] private Sprites spritesPrefab;
        [SerializeField] private GameObject frameBlockPrefab;
        
        private readonly string NAME_BLOCK = "BLOCK";
        private readonly int EMPTY_CELL = -1;
        private Dictionary<string, BlockSingleManager> blockSingleManagers;
        private void Start()
        {
            blockSingleManagers = new Dictionary<string, BlockSingleManager>();
            GameManager.Instance.SetValueGameManager(width,height,EMPTY_CELL);
            SpawnRandomGrid();
            SetSizeMainCamera();
        }
        
        private void SpawnBlock(Sprite sprite,Vector3 spwanPosition,int i,int j)
        {
            GameObject block = Instantiate(frameBlockPrefab,spwanPosition,Quaternion.identity,transform);
            BlockSingleManager blockSingleManager = block.GetComponent<BlockSingleManager>();
            blockSingleManager.SetBlock(sprite,GetNameBlock(i,j),i,j);
            blockSingleManagers.Add(block.name,blockSingleManager);
        }
        
        private void SpawnRandomGrid()
        {
            Vector3 startPosition = new Vector3(-(width - 1) / 2, (height - 1) / 2, 0);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Sprite selectedSprite = null;
                    int randomNumber = EMPTY_CELL;
                    if (i != 0 && i != height - 1 && j != 0 && j != width - 1)
                    {
                        randomNumber = Random.Range(0, spritesPrefab.spritesList.Count);
                        selectedSprite = spritesPrefab.spritesList[randomNumber];
                        
                    }
                    GameManager.Instance.SetMatrix(randomNumber,i,j); 
                    SpawnBlock(selectedSprite,startPosition + new Vector3(j, -i, 0),i,j);
                }
            }
        }

        public void ShuffleGridHandle()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int randomNumber = Random.Range(0, spritesPrefab.spritesList.Count);
                    Sprite selectedSprite = spritesPrefab.spritesList[randomNumber];
                    BlockSingleManager blockSingleManager = blockSingleManagers[GetNameBlock(i,j)];
                    blockSingleManager.SetSpriteBlock(selectedSprite);

                }
            }
        }

        public BlockSingleManager GetBlockSingleManager(Point p)
        {
            string nameBlock = GetNameBlock(p.Y,p.X);
            return blockSingleManagers[nameBlock];
        }
        private string GetNameBlock(int i, int j)
        {
            return NAME_BLOCK + " " + i + "-" + j;
        }
        private void SetSizeMainCamera()
        {
            CameraManager.Instance.ResizeCameraToFitGrid(width, height);
        }
    }
}
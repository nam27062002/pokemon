using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using _Scripts.Block;
using _Scripts.Grid;
using _Scripts.Singleton;
using _Scripts.UI;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int[,] matrix;
    private int width, height;
    private int EMPTY_CELL;
    private BlockSingleManager blockSingleManagerLastClicked = null;
    private List<Vector3> pathDirection;
    private bool isSuggest = false;
    private List<BlockSingleManager> blockSingleManagersSuggest;
    private int clickedValidAmount = 0;
    private void Start()
    {
        blockSingleManagersSuggest = new List<BlockSingleManager>();
        pathDirection = new List<Vector3>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) FindWayValid();
        if (Input.GetKeyDown(KeyCode.A)) StartCoroutine(SolvePuzzle());
    }

    public void SetValueGameManager(int width, int height,int emptyCell)
    {
        this.width = width;
        this.height = height;
        this.EMPTY_CELL = emptyCell;
        matrix = new int[height, width];
    }
    
    public void SetMatrix(int value, int i, int j)
    {
        matrix[i, j] = value;
    }

    public void ClickedBlockHandle(BlockSingleManager blockSingleManager)
    {
        if (isSuggest) UnSuggestDirection();

        if (blockSingleManagerLastClicked == null)
        {
            blockSingleManagerLastClicked = blockSingleManager;
        }
        else
        {
            if (blockSingleManagerLastClicked != blockSingleManager)
            {
                TryMatchBlockSingleManagers(blockSingleManagerLastClicked, blockSingleManager);
                
            }

            blockSingleManagerLastClicked = null;
        }
    }

    private void SetValueMatrix(Point p, int value)
    {
        matrix[p.Y, p.X] = value;
    }
    
    private void TryMatchBlockSingleManagers(BlockSingleManager block1, BlockSingleManager block2)
    {
        Point p1 = block1.PointBlock;
        Point p2 = block2.PointBlock;
        if (IsPathValid(p1, p2))
        {
            SetValueMatrix(p1, EMPTY_CELL);
            SetValueMatrix(p2, EMPTY_CELL);
            block1.Hide();
            block2.Hide();
            DrawLine.Instance.SetLines(pathDirection);
            clickedValidAmount += 2;
        }
        else
        {
            block1.UnSelect();
            block2.UnSelect();
        }
        
        if (IsEndGame())
        {
            Debug.Log("END GAME");
        }
        else if (IsCanMove() == null)
        {
            Debug.Log("LOAD NEW MAP");
        }
    }
    private void TryMatchBlockSingleManagers(Point p1,Point p2)
    {
        BlockSingleManager block1 = GridManager.Instance.GetBlockSingleManager(p1);
        BlockSingleManager block2 = GridManager.Instance.GetBlockSingleManager(p2);
        p1 = block1.PointBlock;
        p2 = block2.PointBlock;
        if (IsPathValid(p1, p2))
        {
            SetValueMatrix(p1, EMPTY_CELL);
            SetValueMatrix(p2, EMPTY_CELL);
            block1.Hide();
            block2.Hide();
            DrawLine.Instance.SetLines(pathDirection);
        }
        else
        {
            block1.UnSelect();
            block2.UnSelect();
        }

        if (IsEndGame())
        {
            Debug.Log("END GAME");
        }
        else if (IsCanMove() == null)
        {
            MainGameManager.Instance.SetArrowActive();
        }
    }
    private bool IsPathValid(Point p1, Point p2)
    {
        if (matrix[p1.Y, p1.X] != matrix[p2.Y, p2.X]) return false;
        
        if (CheckOneLineX(p1, p2))
        {
            return true;
        }
        if (CheckOneLineY(p1, p2))
        {
            return true;
        }
        if (CheckTwoLine(p1, p2))
        {
            return true;
        }
        if (CheckThreeLine(p1, p2))
        {
            return true;
        }
        
        return false;
    }
    
    private bool CheckOneLineX(Point p1, Point p2)
    {
        if (p1.Y != p2.Y) return false;
        int min = Math.Min(p1.X, p2.X);
        int max = Math.Max(p1.X, p2.X);
        if (Enumerable.Range(min + 1, max - min - 1).All(i => matrix[p1.Y, i] == EMPTY_CELL))
        {
            SetPathDirection(new List<Point>(new[] { p1, p2 }));
            return true;
        }
        return false;
    }
    
    private bool CheckOneLineY(Point p1, Point p2)
    {
        if (p1.X != p2.X) return false;
        int min = Math.Min(p1.Y, p2.Y);
        int max = Math.Max(p1.Y, p2.Y);
        if (Enumerable.Range(min + 1, max - min - 1).All(i => matrix[i, p1.X] == EMPTY_CELL))
        {
            SetPathDirection(new List<Point>(new[] { p1, p2 }));
            return true;
        }
        return false;
    }
    
    private bool CheckTwoLine(Point p1, Point p2)
    {
        if (p1.X == p2.X || p1.Y == p2.Y) return false;
    
        Point point1 = new Point(p2.X, p1.Y);
    
        if (CheckOneLineX(p1, point1) && CheckOneLineY(p2, point1) && matrix[point1.Y, point1.X] == EMPTY_CELL)
        {
            SetPathDirection(new List<Point>(new[] { p1, point1, p2 }));
            return true;
        }
        Point point2 = new Point(p1.X, p2.Y);
        if (CheckOneLineY(p1, point2) && CheckOneLineY(point2, p2) && matrix[point2.Y, point2.X] == EMPTY_CELL)
        {
            SetPathDirection(new List<Point>(new[] { p1, point2, p2 }));
            return true;
        }
        return false;
    }
    
    private bool CheckThreeLine(Point p1, Point p2)
    {
        // horizontal
        if (p1.X != p2.X)
        {
            for (int i = 0; i < height; i++)
            {
                if (i != p1.Y && i != p2.Y)
                {
                    Point point1 = new Point(p1.X, i);
                    Point point2 = new Point(p2.X, i);
                    if (CheckOneLineY(p1, point1) && CheckOneLineY(p2, point2) &&
                        CheckOneLineX(point1, point2) && matrix[point1.Y, point1.X] == EMPTY_CELL &&
                        matrix[point2.Y, point2.X] == EMPTY_CELL)
                    {
                        SetPathDirection(new List<Point>(new[] { p1, point1, point2, p2 }));
                        return true;
                    }
                }
            }
        }
    
        // vertical
        if (p1.Y != p2.Y)
        {
            for (int i = 0; i < width; i++)
            {
                if (i != p1.X && i != p2.X)
                {
                    Point point1 = new Point(i, p1.Y);
                    Point point2 = new Point(i, p2.Y);
                    if (CheckOneLineX(p1, point1) && CheckOneLineX(p2, point2) &&
                        CheckOneLineY(point1, point2) && matrix[point1.Y, point1.X] == EMPTY_CELL &&
                        matrix[point2.Y, point2.X] == EMPTY_CELL)
                    {
                        SetPathDirection(new List<Point>(new[] { p1, point1, point2, p2 }));
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
    private void SetPathDirection(List<Point> points)
    {
        pathDirection.Clear();
        foreach (Point p in points)
        {
            pathDirection.Add(GridManager.Instance.GetBlockSingleManager(p).transform.position);
        }
    }

    public void SuggestDirection()
    {
        if (isSuggest) return;
        isSuggest = true;
        List<Point> points = IsCanMove();
        foreach (Point p in points)
        {
            BlockSingleManager blockSingleManager = GridManager.Instance.GetBlockSingleManager(p);
            blockSingleManagersSuggest.Add(blockSingleManager);
            blockSingleManager.SetBlockSuggest(true);
        }
    }

    private void UnSuggestDirection()
    {
        isSuggest = false;
        foreach (BlockSingleManager blockSingleManager in blockSingleManagersSuggest)
        {
            blockSingleManager.SetBlockSuggest(false);
        }
        blockSingleManagersSuggest.Clear();
    }
    private List<Point> IsCanMove()
    {
        for (int i = 1; i < height - 1; i++)
        {
            for (int j = 1; j < width - 1; j++)
            {
                if (matrix[i, j] != EMPTY_CELL)
                {
                    List<Point> matchingPoints = GetMatchingPoints(i, j);
                    foreach (Point point in matchingPoints)
                    {
                        if (IsPathValid(point,new Point(j, i)))
                        {
                            return new List<Point>(new []{point,new Point(j, i)});
                        }
                    }
                }
            }
        }
        return null;
    }
    
    private List<Point> GetMatchingPoints(int ii, int jj)
    {
        List<Point> matchingPoints = new List<Point>();
        for (int i = 1; i < height - 1; i++)
        {
            for (int j = 0; j < width - 1; j++)
            {
                if (matrix[ii, jj] == matrix[i, j] && !(i == ii && j == jj))
                {
                    matchingPoints.Add(new Point(j, i));
                }
            }
        }
    
        return matchingPoints;
    }
    
    private void FindWayValid()
    {
        List<Point> pointsValid = IsCanMove();
        if (pointsValid != null)
        {
            TryMatchBlockSingleManagers(pointsValid[0],pointsValid[1]);
        }
    }

    private bool IsEndGame()
    {
        return clickedValidAmount == (width - 2) * (height - 2);
    }
    
    IEnumerator SolvePuzzle()
    {
        bool puzzleSolved = true;
        while (puzzleSolved)
        {
            puzzleSolved = false;
            for (int i = 1; i < height - 1; i++)
            {
                for (int j = 1; j < width - 1; j++)
                {
                    if (matrix[i, j] != EMPTY_CELL)
                    {
                        List<Point> matchingPoints = GetMatchingPoints(i, j);
                        foreach (Point point in matchingPoints)
                        {
                            if (IsPathValid(point, new Point(j, i)))
                            {
                                TryMatchBlockSingleManagers(point, new Point(j, i));
                                if (!puzzleSolved) puzzleSolved = true;
                                yield return new WaitForSeconds(0.1f);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
    
    

    
}

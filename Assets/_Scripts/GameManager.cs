using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using _Scripts.Grid;
using _Scripts.Pikachu;
using _Scripts.Sound;
using _Scripts.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    // event
    public event EventHandler OnLoadNewMap;
    // 
    private static GameManager instance;
    public static GameManager Instance => instance;
    private const int EMPTY_CELL = -1;
    [SerializeField] private SpawnGrid spawnGrid;
    private int[,] matrix;
    private int width;
    private int height;
    private List<PikachuSingleManager> pikachuSingleManagers;
    private PikachuSingleManager pikachuSingleManagerClicked;
    private List<Vector3> pathDirection;
    private int clickedValidAmount = 0;
    public int ClickedValidAmount => clickedValidAmount;
    private void Awake()
    {
        if (instance == null) instance = this;
        pikachuSingleManagers = new List<PikachuSingleManager>();
        pikachuSingleManagerClicked = null;
        pathDirection = new List<Vector3>();
    }

    private void Start()
    {
        spawnGrid.OnFinishedSpawnGrid += OnFinishedSpawnGrid;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FindWayValid();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SolvePuzzle());
        }
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
    
    private void FindWayValid()
    {
        List<Point> pointsValid = IsCanMove();
        if (pointsValid != null)
        {
            TryMatchPikachuSingleManagers(pointsValid[0], pointsValid[1]);
        }
    }
    
    IEnumerator  SolvePuzzle()
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
                                TryMatchPikachuSingleManagers(point, new Point(j, i));
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

    private void OnFinishedSpawnGrid(object sender, SpawnGrid.FinishedSpawnEventArgs e)
    {
        width = e.GridWidth;
        height = e.GridHeight;
        matrix = e.Matrix;
        pikachuSingleManagers = e.PikachuSingleManagers;
    }

    private void SetPathDirection(List<Point> points)
    {
        pathDirection.Clear();
        foreach (Point p in points)
        {
            pathDirection.Add(GetPikachuSingleManager(p).transform.position);
        }
    }

    private bool IsPathValid(Point p1, Point p2)
    {
        if (matrix[p1.Y, p1.X] != matrix[p2.Y, p2.X]) return false;

        if (CheckOneLineX(p1, p2, true))
        {
            // Debug.Log($"OneLineX {p1} - {p2}");
            return true;
        }
        if (CheckOneLineY(p1, p2, true))
        {
            // Debug.Log($"OneLineY {p1} - {p2}");
            return true;
        }
        if (CheckTwoLine(p1, p2))
        {
            // Debug.Log($"TwoLine {p1} - {p2}");
            return true;
        }
        if (CheckThreeLine(p1, p2))
        {
            // Debug.Log($"ThreeLine {p1} - {p2}");
            return true;
        }

        return false;
    }

    private bool CheckOneLineX(Point p1, Point p2, bool isSetPath = false)
    {
        if (p1.Y != p2.Y) return false;
        int min = Math.Min(p1.X, p2.X);
        int max = Math.Max(p1.X, p2.X);
        if (Enumerable.Range(min + 1, max - min - 1).All(i => matrix[p1.Y, i] == EMPTY_CELL))
        {
            if (isSetPath) SetPathDirection(new List<Point>(new[] { p1, p2 }));
            return true;
        }
        return false;
    }

    private bool CheckOneLineY(Point p1, Point p2, bool isSetPath = false)
    {
        if (p1.X != p2.X) return false;
        int min = Math.Min(p1.Y, p2.Y);
        int max = Math.Max(p1.Y, p2.Y);
        if (Enumerable.Range(min + 1, max - min - 1).All(i => matrix[i, p1.X] == EMPTY_CELL))
        {
            if (isSetPath) SetPathDirection(new List<Point>(new[] { p1, p2 }));
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

    private PikachuSingleManager GetPikachuSingleManager(Point p)
    {
        foreach (PikachuSingleManager pikachuSingleManager in pikachuSingleManagers)
        {
            if (pikachuSingleManager.Point == p)
            {
                return pikachuSingleManager;
            }
        }

        return null;
    }

    private void TryMatchPikachuSingleManagers(Point point1, Point point2)
    {
        PikachuSingleManager p1 = GetPikachuSingleManager(point1);
        PikachuSingleManager p2 = GetPikachuSingleManager(point2);
        if (IsPathValid(p1.Point, p2.Point))
        {
            // valid
            matrix[p1.Point.Y, p1.Point.X] = EMPTY_CELL;
            matrix[p2.Point.Y, p2.Point.X] = EMPTY_CELL;
            p1.Hide();
            p2.Hide();
            DrawLine.Instance.SetLines(pathDirection);
            SoundManager.Instance.PlaySoundSuccess();
            clickedValidAmount += 2;
        }
        else
        {
            p1.SetActiveSelected();
            p2.SetActiveSelected();
            SoundManager.Instance.PlaySoundFail();
        }

        if (IsFinished())
        {
            Debug.Log("FINISHED");
        }
        else if (IsCanMove() == null)
        {
            OnLoadNewMap?.Invoke(this,EventArgs.Empty);
        }
    }

    public void SetPointClicked(PikachuSingleManager pikachuSingleManager)
    {
        if (pikachuSingleManagerClicked == null)
        {
            pikachuSingleManagerClicked = pikachuSingleManager;
            SoundManager.Instance.PlaySoundClick();
        }
        else
        {
            if (pikachuSingleManagerClicked != pikachuSingleManager)
            { 
                TryMatchPikachuSingleManagers(pikachuSingleManagerClicked.Point, pikachuSingleManager.Point);
            }
            pikachuSingleManagerClicked = null;
        }
    }

    private bool IsFinished()
    {
        return clickedValidAmount == (height - 2) * (width - 2);
    }
}

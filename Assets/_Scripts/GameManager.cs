using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using _Scripts.Grid;
using _Scripts.Pikachu;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;
    
    [SerializeField] private SpawnGrid spawnGrid;
    private int[,] matrix;
    private int width;
    private int height;
    private List<PikachuSingleManager> pikachuSingleManagers;
    private PikachuSingleManager pikachuSingleManagerClicked;
    private void Awake()
    {
        if (instance == null) instance = this;
        pikachuSingleManagers = new List<PikachuSingleManager>();
        pikachuSingleManagerClicked = null;
    }

    private void Start()
    {
        spawnGrid.OnFinishedSpawnGrid += SpawnGridOnOnFinishedSpawnGrid;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AI();
        }
    }

    private List<Point> GetListPoint(int ii, int jj)
    {
        List<Point> p = new List<Point>();
        for (int i = 1; i < height - 1; i++)
        {
            for (int j = 0; j < width - 1; j++)
            {
                if (matrix[ii, jj] == matrix[i, j] && !(i == ii && j == jj))
                {
                    p.Add(new Point(j,i));
                } 
            }
        }

        return p;
    }
    private void AI()
    {
        bool running = true;
        while (running)
        {
            running = false;
            for (int i = 1; i < height - 1; i++)
            {
                for (int j = 1; j < width - 1; j++)
                {
                    if (matrix[i, j] != -1)
                    {
                        List<Point> p = GetListPoint(i,j);
                        foreach (Point point in p)
                        {
                            if (ClickHandle(point, new Point(j, i)))
                            {
                                if (!running) running = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
        
    }
    private void ShowMatrix()
    {
        for (int i = 0; i < height; i++)
        {
            string s = "";
            for (int j = 0; j < width; j++)
            {
                if (matrix[i, j].ToString().Length == 1)
                {
                    s += "0" + matrix[i, j] + " ";
                }
                else
                {
                    s += matrix[i, j] + " ";
                }
            }
            Debug.Log(s);
        }
    }
    
    private void SpawnGridOnOnFinishedSpawnGrid(object sender, SpawnGrid.FinishedSpawnEventArgs e)
    {
        width = e.GridWidth;
        height = e.GridHeight;
        matrix = e.Matrix;
        pikachuSingleManagers = e.PikachuSingleManagers;
    }

    private bool IsPathValid(Point p1, Point p2)
    {
        if (matrix[p1.Y, p1.X] != matrix[p2.Y, p2.X]) return false;
        bool check = CheckOneLineX(p1, p2);
        if (!check)
        {
            check = CheckOneLineY(p1, p2);
            if (!check)
            {
                check = CheckTwoLine(p1, p2);
                if (!check)
                {
                    
                }
                else
                {
                    Debug.Log($"TwoLine {p1} - {p2}");

                }
            }
            else
            {
                Debug.Log($"OneLineY {p1} - {p2}");
            }
        }
        else
        {
            Debug.Log($"OneLineX {p1} - {p2}");
        }
        return check;
    }

    private bool CheckOneLineX(Point p1, Point p2)
    {
        if (p1.Y != p2.Y) return false;
        int min = Math.Min(p1.X, p2.X);
        int max = Math.Max(p1.X, p2.X);
        return Enumerable.Range(min + 1, max - min - 1).All(i => matrix[p1.Y, i] == -1);
    }
    
    private bool CheckOneLineY(Point p1, Point p2)
    {
        if (p1.X != p2.X) return false;
        int min = Math.Min(p1.Y, p2.Y);
        int max = Math.Max(p1.Y, p2.Y);
        return Enumerable.Range(min + 1, max - min - 1).All(i => matrix[i, p1.X] == -1);
    }

    private bool CheckTwoLine(Point p1, Point p2)
    {
        if (p1.X == p2.X || p1.Y == p2.Y) return false;
        Point point1 = new Point(p2.X,p1.Y);
        if (CheckOneLineX(p1, point1) && CheckOneLineY(p2, point1) && matrix[point1.Y, point1.X] == -1)
        {
            return true;
        }
        Point point2 = new Point(p1.X, p2.Y);
        if (CheckOneLineY(p1,point2) && CheckOneLineY(point2,p2) && matrix[point2.Y,point2.X] == -1)
        {
            return true;
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
    
    private bool ClickHandle(PikachuSingleManager p1,PikachuSingleManager p2)
    {
        if(IsPathValid(p1.Point,p2.Point))
        {
            // valid
            matrix[p1.Point.Y, p1.Point.X] = -1;
            matrix[p2.Point.Y, p2.Point.X] = -1;
            p1.OnDestroy();
            p2.OnDestroy();
            return true;
        }
        else
        {
            p1.SetActiveSelected();
            p2.SetActiveSelected();
            return false;
        }
    }
    
    private bool ClickHandle(Point point1,Point point2)
    {
        PikachuSingleManager p1 = GetPikachuSingleManager(point1);
        PikachuSingleManager p2 = GetPikachuSingleManager(point2);
        if(IsPathValid(p1.Point,p2.Point))
        {
            // valid
            matrix[p1.Point.Y, p1.Point.X] = -1;
            matrix[p2.Point.Y, p2.Point.X] = -1;
            p1.OnDestroy();
            p2.OnDestroy();
            return true;
        }
        else
        {
            p1.SetActiveSelected();
            p2.SetActiveSelected();
            return false;
        }
    }
    
    public void SetPointClicked(PikachuSingleManager pikachuSingleManager)
    {
        if (pikachuSingleManagerClicked == null)
        {
            pikachuSingleManagerClicked = pikachuSingleManager;
        }
        else
        {
            if (pikachuSingleManagerClicked != pikachuSingleManager)
            {
                ClickHandle(pikachuSingleManagerClicked,pikachuSingleManager);
            }
        }
    }
    
}

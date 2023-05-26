using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class GridBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public int rows = 10;
    public int columns = 10;
    public int scale = 1;
    public GameObject gridPrefab;
    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);

    public GameObject[,] gridArray;
    public int startX = 0;
    public int startZ = 0;
    public int endX = 2;
    public int endZ = 2;

    public List<GameObject> path = new List<GameObject>();
    void Start()
    {
        gridArray = new GameObject[rows, columns];
        if (gridPrefab)
        {
            GenerateGrid();
        }
        else
        {
            print("Grid Prefab");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateGrid()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int z = 0; z < columns; z++)
            {
                Vector3 pos = new Vector3(
                    leftBottomLocation.x+(x*scale), leftBottomLocation.y, leftBottomLocation.z+(z*scale));

                GameObject obj = Instantiate(gridPrefab, pos, Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                obj.GetComponent<GridStat>().x = x;
                obj.GetComponent<GridStat>().z = z;
                gridArray[x, z] = obj;
            }
        }
    }

    void InitSetup()
    {
        foreach (GameObject obj in gridArray)
        {
            obj.GetComponent<GridStat>().visited = -1;
        }
        gridArray[startX, startZ].GetComponent<GridStat>().visited = 0;
    }

    bool TestDirection(int x, int z, int step, int direction)
    {
        switch (direction)
        {
            // can go up
            case 1:

                if (z+1<columns && gridArray[x, z+1] && gridArray[x, z+1].GetComponent<GridStat>().visited==step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            // can go right
            case 2:

                if (x+1<rows && gridArray[x+1, z] && gridArray[x+1, z].GetComponent<GridStat>().visited==step)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            // can go down ;)
            case 3:

                if (z-1>-1 && gridArray[x, z-1] && gridArray[x, z-1].GetComponent<GridStat>().visited==step)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            // can go left
            case 4:

                if (x-1<-1 && gridArray[x-1, z] && gridArray[x-1, z].GetComponent<GridStat>().visited==step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
        return false;
    }

    void SetDistance()
    {
        InitSetup();
        int x = startX;
        int z = startZ;
        int[] testArray = new int[rows * columns];
        for (int step = 1; step < columns; step++)
        {
            foreach (GameObject obj in gridArray)
            {
                GridStat stat = obj.GetComponent<GridStat>();
                if (stat.visited == step-1)
                {
                    TestFourDirection(stat.x, stat.z, step);
                }
            }
        }
    }

    void SetPath()
    {
        int step;
        int x = endX;
        int z = endZ;
        List<GameObject> tempList = new List<GameObject>();
        path.Clear();
        if (gridArray[endX, endZ] && gridArray[endX, endZ].GetComponent<GridStat>().visited>0)
        {
            path.Add(gridArray[endX, endZ]);
            step = gridArray[x, z].GetComponent<GridStat>().visited-1;
        }
        else
        {
            print("Can't get there");
            return;
        }

        for (int i = step; step > -1; step--)
        {
            if (TestDirection(x, z, step, 1))
            {
                tempList.Add(gridArray[x, z+1]);
            }
            if (TestDirection(x, z, step, 2))
            {
                tempList.Add(gridArray[x+1, z]);
            }
            if (TestDirection(x, z, step, 3))
            {
                tempList.Add(gridArray[x, z-1]);
            }
            if (TestDirection(x, z, step, 4))
            {
                tempList.Add(gridArray[x-1, z]);
            }

        }
    }

    void TestFourDirection(int x, int z, int step)
    {
        if (TestDirection(x, z, -1, 1))
        {
            SetVisited(x, z+1, step);
        }
        if (TestDirection(x, z, -1, 2))
        {
            SetVisited(x+1, z, step);
        }
        if (TestDirection(x, z, -1, 3))
        {
            SetVisited(x, z-1, step);
        }
        if (TestDirection(x, z, -1, 4))
        {
            SetVisited(x-1, z, step);
        }

    }

    void SetVisited(int x, int z, int step)
    {
        if (gridArray[x, z])
        {
            gridArray[x, z].GetComponent<GridStat>().visited = step;
        }
    }
}

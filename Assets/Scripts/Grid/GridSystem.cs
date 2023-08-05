using UnityEditor;
using UnityEngine;
using System;
public class GridSystem<TGridObject>
{
    private int width;
    private int height;
    private float cellSize;
    private TGridObject[,] gridObjects;

    public GridSystem(int width, int height , float cellSize , Func<GridSystem<TGridObject> , GridPosition , TGridObject> createGridObject) 
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjects = new TGridObject[width, height];

        for(int x = 0; x < width; x++) 
        {
            for(int z = 0; z < height; z++)
            {
                gridObjects[x,z] = createGridObject(this,new GridPosition(x,z));
            }
        }
    }


    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x , 0 , gridPosition.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(Mathf.RoundToInt(worldPosition.x / cellSize), Mathf.RoundToInt(worldPosition.z / cellSize));
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjects[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.z >= 0
            && gridPosition.x < width && gridPosition.z < height;
    }

    public void CreateDebugPrefabs(Transform debugPrefab , Transform parent)
    {


        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform debugGameObject = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity , parent);

                debugGameObject.GetComponent<GridDebugObject>().SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

}

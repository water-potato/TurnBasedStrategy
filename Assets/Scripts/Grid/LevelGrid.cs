using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int cellSize;


    [SerializeField] Transform gridDebugPrefab;

    GridSystem<GridObject> gridSystem;


    public event EventHandler OnAnyUnitMovedGridPosition;
    void Awake()
    {
        gridSystem = new GridSystem<GridObject>(width, height, cellSize, 
            (GridSystem<GridObject> g , GridPosition gridPosition) => new GridObject(g, gridPosition) );
        //gridSystem.CreateDebugPrefabs(gridDebugPrefab);

        if(Instance != null)
        {
            Debug.LogError("There's more than one LevelGrid!" + transform + '-' + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        Pathfinding.Instance.Setup(width , height , cellSize);
    }
    public void AddUnitAtGridPosition(GridPosition gridPosition , Unit unit)
    {
        gridSystem.GetGridObject(gridPosition).AddUnit(unit);
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition , Unit unit)
    {
        gridSystem.GetGridObject(gridPosition).RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(GridPosition fromGridPosition, GridPosition toGridPosition ,  Unit unit)
    {
        RemoveUnitAtGridPosition(fromGridPosition , unit);
        AddUnitAtGridPosition(toGridPosition , unit);

        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();
    public int GetHeight() => gridSystem.GetHeight();

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.Interactable;
    }
    public void SetInteractableAtGridPosition(GridPosition gridPosition , IInteractable interactable)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.Interactable = interactable;
    }
}
    
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] Transform gridDebugPrefab;
    GridSystem gridSystem;

    public static LevelGrid Instance { get; private set; }


    void Awake()
    {
        gridSystem = new GridSystem(10, 10, 2);
        gridSystem.CreateDebugPrefabs(gridDebugPrefab);

        if(Instance != null)
        {
            Debug.LogError("There's more than one LevelGrid!" + transform + '-' + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition , Unit unit)
    {
        gridSystem.GetGridObject(gridPosition).AddUnit(unit);
    }

    public List<Unit> GetUnitAtGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition , Unit unit)
    {
        gridSystem.GetGridObject(gridPosition).RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(GridPosition fromGridPosition, GridPosition toGridPosition ,  Unit unit)
    {
        RemoveUnitAtGridPosition(fromGridPosition , unit);
        AddUnitAtGridPosition(toGridPosition , unit);
    }

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();
    public int GetHeight() => gridSystem.GetHeight();

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);
}
    
using System.Collections.Generic;
using Unity.VisualScripting;

public class GridObject
{
    private GridSystem gridSystem;
    private GridPosition gridPosition;
    public List<Unit> unitList;
    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;

        unitList = new List<Unit>();
    }
    
    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public bool HasAnyUnit()
    {
        return unitList.Count > 0;
    }



    public override string ToString()
    {
        string objStr = gridPosition.ToString();
        foreach (Unit unit in unitList)
        {
            objStr += "\n" + unit.ToString();
        }

        return objStr;
    }
}

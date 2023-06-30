using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    protected int actionCost = 1;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetName();

    public abstract void TakeAction(GridPosition gridPosition, Action onCompleteAction);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }
    public virtual int GetActionCost()
    {
        return actionCost;
    }
    public abstract List<GridPosition> GetValidGridPositionList();
}

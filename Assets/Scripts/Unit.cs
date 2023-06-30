using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;

    private GridPosition gridPosition;
    private int actionPoints = 2;

    private void Awake()
    {
        moveAction= GetComponent<MoveAction>();
        spinAction= GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

    }
    private void Update()
    {

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(gridPosition, newGridPosition, this);
            gridPosition = newGridPosition;
        }

    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }
    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPoints(BaseAction action)
    {
        if (CanSpendActionPoints(action))
        {
            actionPoints -= action.GetActionCost();
            return true;
        }
        return false;
    }

    public bool CanSpendActionPoints(BaseAction action)
    {
        return actionPoints >= action.GetActionCost();
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }
}

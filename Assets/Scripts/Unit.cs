using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    GridPosition gridPosition;
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
        return GetComponent<MoveAction>();
    }
    public SpinAction GetSpinAction()
    {
        return GetComponent<SpinAction>();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpinAmount;

    private void Start()
    {
        actionCost = 2;
    }
    // Update is called once per frame
    void Update()
    {
        if (isActive == false)
            return;

        float spinAngle = 360f * Time.deltaTime;
        
        transform.eulerAngles += new Vector3(0, spinAngle, 0);
        totalSpinAmount += spinAngle;

        if(totalSpinAmount> 360f)
        {
            ActionComplete();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete )
    {
        totalSpinAmount = 0f;

        ActionStart(onActionComplete);
    }



    public override string GetName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return new List<GridPosition> { unitGridPosition };
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
}

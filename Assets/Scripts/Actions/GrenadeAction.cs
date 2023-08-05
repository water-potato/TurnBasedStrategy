using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    [SerializeField] private Transform grenadeProjectilePrefab;

    private int maxThrowDistance = 7;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
    public override string GetName()
    {
        return "Grenade";
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        List<GridPosition> validGridPositions = new List<GridPosition>();

        for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
        {
            for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);

                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (LevelGrid.Instance.IsValidGridPosition(testGridPosition) == false)
                {
                    // 그리드 위가 아님
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxThrowDistance)
                {
                    continue;
                }


                validGridPositions.Add(testGridPosition);
            }
        }
        return validGridPositions;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Transform grenadeProjectileTransform = 
            Instantiate(grenadeProjectilePrefab , unit.transform.position , Quaternion.identity) ;
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition , OnGrenadeBehaviourComplete);

        ActionStart(onActionComplete);

    }

    private void OnGrenadeBehaviourComplete()
    {
        ActionComplete();
    }

}

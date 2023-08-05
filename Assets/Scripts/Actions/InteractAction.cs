using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    private void Start()
    {
        actionCost = 0;
    }
    private void Update()
    {
        if(isActive == false)
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
    public int MaxInteractDistance { get; private set; } = 1;

    public override string GetName()
    {
        return "Interact";
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        List<GridPosition> validGridPositions = new List<GridPosition>();

        for (int z = -MaxInteractDistance; z <= MaxInteractDistance; z++)
        {
            for (int x = -MaxInteractDistance; x <= MaxInteractDistance; x++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);

                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (LevelGrid.Instance.IsValidGridPosition(testGridPosition) == false)
                {
                    // �׸��� ���� �ƴ�
                    continue;
                }

                IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(testGridPosition);
                if(interactable == null)
                {
                    // �� ��ġ�� ���� ������
                    continue;
                }

                validGridPositions.Add(testGridPosition);
            }
        }
        return validGridPositions;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);
        interactable.Interact(OnInterActComplete);
        ActionStart(onActionComplete);
    }

    public void OnInterActComplete()
    {
        ActionComplete();
    }
}

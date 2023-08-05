using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class MoveAction : BaseAction
{

    [SerializeField] int maxMovementRange;

    // �̵� ���� ������
    List<Vector3> positionList;
    public int currentPositionIndex;

    float movementSpeed = 5f;
    const float ROTATE_SPEED = 50f;
    const float STOPPING_DISTANCE = .1f;

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    // Update is called once per frame
    void Update()
    {
        if(isActive == false)
        {
            return;
        }

        Vector3 targetPosition = positionList[currentPositionIndex];

        if (Vector3.Distance(transform.position, targetPosition) > STOPPING_DISTANCE)
        {
            Vector3 movementVector = (targetPosition - transform.position).normalized;
            transform.position += movementVector * movementSpeed * Time.deltaTime;

            transform.forward = Vector3.Lerp(transform.forward, movementVector, ROTATE_SPEED * Time.deltaTime);
        }
        else
        {
            currentPositionIndex++;
            if(currentPositionIndex >= positionList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
        }
    }

    public override void TakeAction(GridPosition targetPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPosition(), targetPosition , out int pathLength);

        currentPositionIndex = 0;

        positionList = new List<Vector3>();

        foreach(GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }

        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }


    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();

        for(int z =-maxMovementRange; z<=maxMovementRange; z++)
        {
            for(int x = -maxMovementRange; x<=maxMovementRange; x++)
            {
                GridPosition unitGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
                GridPosition testPosition = new GridPosition(x , z);

                testPosition += unitGridPosition;
                if (LevelGrid.Instance.IsValidGridPosition(testPosition) == false)
                {
                    continue;
                }
                if(testPosition == unitGridPosition)
                {
                    // �������� �� ��ġ�� ����
                    continue;
                }
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testPosition))
                {
                    // �ش� ��ġ�� ���� ����
                    continue;
                }

                if(Pathfinding.Instance.IsWalkableGridPosition(testPosition) == false)
                {
                    // �ش� ��ġ�� ��ֹ��� ����
                    continue;
                }
                if (Pathfinding.Instance.HasPath(unit.GetGridPosition() , testPosition) == false)
                {
                    // ������ ��ΰ� ����
                    continue;
                }

                if(Pathfinding.Instance.GetPathLength(unit.GetGridPosition() , testPosition)
                    > maxMovementRange * 10) // A*�� ���� ����ġ�� 10�̴�.
                {
                    // �Ÿ��� �ʹ� �ִ�
                    continue;
                }


                validGridPositions.Add(testPosition);
            }
        }
        return validGridPositions;
    }

    public override string GetName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        // �ش� ��ġ���� Shoot�� ������ ����
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);



        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }
}

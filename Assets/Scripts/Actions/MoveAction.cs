using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class MoveAction : BaseAction
{

    [SerializeField] int maxMovementRange;

    // 이동 관련 변수들
    Vector3 targetPosition;
    float movementSpeed = 3f;
    const float ROTATE_SPEED = 50f;
    const float STOPPING_DISTANCE = .1f;

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;



    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if(isActive == false)
        {
            return;
        }

        if (Vector3.Distance(transform.position, targetPosition) > STOPPING_DISTANCE)
        {
            Vector3 movementVector = (targetPosition - transform.position).normalized;
            transform.position += movementVector * movementSpeed * Time.deltaTime;

            transform.forward = Vector3.Lerp(transform.forward, movementVector, ROTATE_SPEED * Time.deltaTime);
        }
        else
        {
            OnStopMoving?.Invoke(this, EventArgs.Empty);
            ActionComplete();
        }
    }

    public override void TakeAction(GridPosition targetPosition , Action onActionComplete)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
        ActionStart(onActionComplete);

        OnStartMoving?.Invoke(this, EventArgs.Empty);
    }


    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();

        for(int z =-maxMovementRange; z<=maxMovementRange; z++)
        {
            for(int x = -maxMovementRange; x<=maxMovementRange; x++)
            {
                GridPosition unitGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
                GridPosition validPosition = new GridPosition(x , z);

                validPosition += unitGridPosition;
                if (LevelGrid.Instance.IsValidGridPosition(validPosition) == false)
                {
                    continue;
                }
                if(validPosition == unitGridPosition)
                {
                    // 포지션이 내 위치와 같음
                    continue;
                }
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(validPosition))
                {
                    // 해당 위치에 적이 있음
                    continue;
                }

                validGridPositions.Add(validPosition);
            }
        }
        return validGridPositions;
    }

    public override string GetName()
    {
        return "Move";
    }
}

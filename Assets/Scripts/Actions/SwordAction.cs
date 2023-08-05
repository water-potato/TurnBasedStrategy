using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    public static event EventHandler OnAnySwordHit;

    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted;

    private enum State
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit,
    }
    public int MaxSwordDistance { get; private set; } = 1;
    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private void Update()
    {
        if(isActive == false)
        {
            return;
        }

        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.SwingingSwordBeforeHit:

                Vector3 aimDir = (targetUnit.transform.position - transform.position).normalized;
                const float ROTATE_SPEED = 30f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, ROTATE_SPEED * Time.deltaTime);
                break;
            case State.SwingingSwordAfterHit:
                break;

        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                state = State.SwingingSwordAfterHit;
                float afterHitStateTime = 0.5f;
                stateTimer = afterHitStateTime;
                targetUnit.Damage(100);
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200,
        };
    }

    public override string GetName()
    {
        return "Sword";
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        List<GridPosition> validGridPositions = new List<GridPosition>();

        for (int z = -MaxSwordDistance; z <= MaxSwordDistance; z++)
        {
            for (int x = -MaxSwordDistance; x <= MaxSwordDistance; x++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);

                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (LevelGrid.Instance.IsValidGridPosition(testGridPosition) == false)
                {
                    // 그리드 위가 아님
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition) == false)
                {
                    // 해당 위치에 적이 없음
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    // 해당 유닛이 본인과 같은 팀임
                    continue;
                }


                validGridPositions.Add(testGridPosition);
            }
        }
        return validGridPositions;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        state = State.SwingingSwordBeforeHit;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        OnSwordActionStarted?.Invoke(this , EventArgs.Empty);

        ActionStart(onActionComplete);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }
    private const float ROTATE_SPEED = 30f;

    private State state;
    private int maxShootDistance = 7;
    private float stateTimer;

    private Unit targetUnit;
    private bool canShootBullet;



    private void Update()
    {
        if(!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.Aiming:

                Vector3 aimDir = (targetUnit.transform.position - transform.position).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, ROTATE_SPEED * Time.deltaTime);
                break;
            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;

        }

        if(stateTimer <= 0f)
        {
            NextState();
        }
    }


    private void NextState()
    {
        switch(state)
        {
            case State.Aiming:
                    state = State.Shooting;
                    float shootingStateTime = 0.1f;
                    stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                    state = State.Cooloff;
                    float coolOffStateTime = 0.5f;
                    stateTimer = coolOffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit

        });
        targetUnit.Damage(40);
    }
      
    public override string GetName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();

        for (int z = -maxShootDistance; z <= maxShootDistance; z++)
        {
            for (int x = -maxShootDistance; x <= maxShootDistance; x++)
            {
                GridPosition unitGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
                GridPosition validPosition = new GridPosition(x, z);

                validPosition += unitGridPosition;
                if (LevelGrid.Instance.IsValidGridPosition(validPosition) == false)
                {
                    // 그리드 위가 아님
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > maxShootDistance)
                {
                    continue;
                }


                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(validPosition) == false)
                {
                    // 해당 위치에 적이 없음
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(validPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    // 해당 유닛이 본인과 같은 팀임
                    continue;
                }

                validGridPositions.Add(validPosition);
            }
        }
        return validGridPositions;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        canShootBullet = true;

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        
    }
}
  
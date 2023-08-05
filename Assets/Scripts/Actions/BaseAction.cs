using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler   OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    protected int actionCost = 1;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;

        OnAnyActionStarted?.Invoke(this , EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();

        OnAnyActionCompleted?.Invoke(this , EventArgs.Empty);
    }



    public abstract string GetName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }
    public virtual int GetActionCost()
    {
        return actionCost;
    }

    public Unit GetUnit()
    {
        return unit;
    }


    /// <summary>
    /// �ش� action�� valid �� ��ġ�� ���� AI value�� �����ͼ�, ���� Best�� ����.
    /// </summary>
    public EnemyAIAction GetBestEnemyAIAction() 
    {
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction> ();

        List<GridPosition> validActionGridPositionList = GetValidGridPositionList ();

        foreach(GridPosition gridPosition in validActionGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActionList.Add(enemyAIAction);
        }
        if (enemyAIActionList.Count > 0)
        {
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue); // ������������ �����ϴ� �޼ҵ�
            return enemyAIActionList[0];
        }
        else
        {
            //No Possible Enemy AI Actions
            return null;
        }
    }

    public abstract EnemyAIAction GetEnemyAIAction (GridPosition gridPosition);
    public abstract List<GridPosition> GetValidGridPositionList();
}

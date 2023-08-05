using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{


    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }


    private State state;
    private float timer;

    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }
    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }
    private void Update()
    {
        if(TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (state)
        {

            case State.WaitingForEnemyTurn:
                break;
            case 
                State.TakingTurn:
                timer -= Time.deltaTime;

                if (timer <= 0f)
                {
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        //������ �׼�����Ʈ�� ���� �Ҹ�
                        TurnSystem.Instance.NextTurn();
                     }
                }
                break;
            case State.Busy: 
                break;

        }

    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {

        if(TurnSystem.Instance.IsPlayerTurn() == false)
        {
            state = State.TakingTurn;
            timer = 2f;

        }
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        foreach(Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if(TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// �ش� ������ ��� �ൿ���� BestAction�� �����ͼ� ���Ѵ�.
    /// </summary>
    private bool TryTakeEnemyAIAction(Unit enemyUnit , Action onEnemyAIActionComplete) 
    {


        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;


        foreach (BaseAction baseAction in enemyUnit.GetBaseActionArray())
        {
            if (!enemyUnit.CanSpendActionPoints(baseAction))
            {
                // ���� �� �ൿ�� �� �� ������
                continue;
            }

            if (bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if(testEnemyAIAction != null && testEnemyAIAction.actionValue> bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                    bestBaseAction = baseAction;
                }
            }
        }


        if(bestEnemyAIAction != null && enemyUnit.TrySpendActionPoints(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }

        return false;

    }

}
 
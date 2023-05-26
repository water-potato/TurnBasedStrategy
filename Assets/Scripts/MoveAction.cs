using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class MoveAction : MonoBehaviour
{

    [SerializeField] Animator unitAnimator;
    [SerializeField] int maxMovementRange;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
    }

    // 이동 관련 변수들
    Vector3 targetPosition;
    float movementSpeed = 3f;
    const float ROTATE_SPEED = 15f;
    const float STOPPING_DISTANCE = .1f;

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(transform.position, targetPosition) > STOPPING_DISTANCE)
        {
            Vector3 movementVector = (targetPosition - transform.position).normalized;
            transform.position += movementVector * movementSpeed * Time.deltaTime;
            unitAnimator.SetBool("IsWalking", true);

            transform.forward = Vector3.Lerp(transform.forward, movementVector, ROTATE_SPEED * Time.deltaTime);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
        }
    }

    public void Move(GridPosition targetPosition)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidGridPositionList()
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
}

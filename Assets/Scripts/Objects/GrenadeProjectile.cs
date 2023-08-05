using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded;

    [SerializeField] private Transform greandeExplodeVfxPrefab;
    [SerializeField] private AnimationCurve arcYAnimationCurve;

    private Vector3 targetWorldPosition;
    private Action OnGrenadeBehaviourComplete;
    private float totalDistance;
    private Vector3 positionXZ;
    private void Update()
    {
        Vector3 moveDir = (targetWorldPosition - positionXZ).normalized;
        float moveSpeed = 15f;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        // y축 커브 만들기

        float distance = Vector3.Distance(positionXZ, targetWorldPosition);
        float distanceNoarmalized = 1- (distance / totalDistance);

        float maxHeight = totalDistance / 3f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNoarmalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x , positionY , positionXZ.z);

        float reachedTargetDistance = .2f;
        if(Vector3.Distance(positionXZ, targetWorldPosition) < reachedTargetDistance)
        {
            float damageRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(targetWorldPosition, damageRadius);

            foreach(Collider collider in colliderArray)
            {
                if(collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(30);
                }
                if (collider.TryGetComponent<DestructibleCrate>(out DestructibleCrate DestructableCrate))
                {
                    DestructableCrate.Damage();
                }
            }
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);

            Instantiate(greandeExplodeVfxPrefab , targetWorldPosition + Vector3.up , Quaternion.identity);
            Destroy(gameObject);

            OnGrenadeBehaviourComplete();
        }
    }
    public void Setup(GridPosition targetGridPosition , Action OnGrenadeBehaviourComplete)
    {
        this.OnGrenadeBehaviourComplete = OnGrenadeBehaviourComplete;
        targetWorldPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetWorldPosition);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }


    [SerializeField] private Unit selectedUnit;
    [SerializeField] LayerMask unitLayerMask = 1 << 7;

    private bool isBusy;

    public event EventHandler OnSelectedUnitChanged;
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There's more than one UnitActionSystem!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {

        if (isBusy == true)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (HandleUnitSelection() == true)
            {
                return;
            }

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetMousePoisiion());

            if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            {
                selectedUnit.GetMoveAction().Move(mouseGridPosition , EndBusy);
                SetBusy();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            selectedUnit.GetSpinAction().Spin(EndBusy);
            SetBusy();
        }
    }

    private void SetBusy()
    {
        isBusy = true;
    }
    private void EndBusy()
    {
        isBusy = false;
    }

   

    private bool HandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, unitLayerMask) == true)
        {
            if (hit.transform.TryGetComponent<Unit>(out selectedUnit) == false)
            {
                Debug.Log("Failed to Get Unit");
            }
            else
            {
                OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
                return true;
            }
            }
        return false;
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}

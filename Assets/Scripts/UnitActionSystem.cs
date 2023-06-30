using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }


    [SerializeField] private Unit selectedUnit;
    [SerializeField] LayerMask unitLayerMask = 1 << 7;

    private BaseAction selectedAction;

    private bool isBusy;

    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;
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

    public void Start()
    {
        SetSelectedUnit(selectedUnit);
    }
    void Update()
    {
        if (isBusy == true) // 행동 중이면
            return;

        if (EventSystem.current.IsPointerOverGameObject()) // 마우스가 UI 위에 있으면
            return;

        if (HandleUnitSelection() == true) // 유닛을 골랐으면
        {
            return;
        }
        HandleSecetedAction();
    }


    private void HandleSecetedAction()
    {

        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetMousePoisiion());
            if (selectedAction.IsValidActionGridPosition(mouseGridPosition) == false)
            {
                // 유효한 GridPosition이 아님
                return;
            }

            if (selectedUnit.TrySpendActionPoints(selectedAction) == false)
            {
                // actionPoint가 부족함
                return;
            }
            selectedAction.TakeAction(mouseGridPosition, EndBusy);
            SetBusy();
            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
    }
    private void EndBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }

   

    private bool HandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, unitLayerMask) == true)
            {
                if (hit.transform.TryGetComponent<Unit>(out Unit unit) == true)
                {
                    if(unit == selectedUnit)
                    {
                        //같은 유닛을 고름
                        return false;
                    }
                    SetSelectedUnit(unit);
                    return true;
                }

            }
        }
        return false;
    }
    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction= baseAction;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
}
